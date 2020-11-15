// https://www.raywenderlich.com/7728186-creating-a-replay-system-in-unity

using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class GhostReplaySystem : MonoBehaviour
{
    [SerializeField]
    FlyingWing craft;
    
    [SerializeField]
    FlyingWingGhost ghost;

    //----------------------------------------------------------------------------------------------------
    
    public bool IsRecording => isRecording;

    public bool HasReplay => hasReplay;
    
    public bool IsReplaying => isReplaying;
    
    public void StartRecording()
    {
        isRecording = true;

        if( binaryWriter == null )
        {
            binaryWriter = new BinaryWriter( new MemoryStream() );
        }
        else
        {
            binaryWriter.Seek( 0, SeekOrigin.Begin );
            binaryWriter.BaseStream.SetLength( 0 );
        }

        recordingTime = 0f;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void SaveReplay()
    {
        if( binaryWriter == null )
        {
            return;
        }

        var writerStream = (MemoryStream)binaryWriter.BaseStream;
        var replayData = writerStream.ToArray();
        
        binaryReader?.Close();
        binaryReader = new BinaryReader( new MemoryStream( replayData ) );

        hasReplay = true;
        
        Task.Run( () => SaveToFileAsync( replayData, replayFilePath ) );
    }

    // Temp
    public void LoadReplay()
    {
        if( !File.Exists( replayFilePath ) )
        {
            return;
        }
        
        var task = Task.Run( () => LoadFromFileAsync( replayFilePath ) );
        task.GetAwaiter().OnCompleted( () =>
        {
            var replayData = task.Result;
            
            binaryReader?.Close();
            binaryReader = new BinaryReader( new MemoryStream( replayData ) );
            
            hasReplay = true;
        } );
    }
    
    public void StartReplaying()
    {
        if( !hasReplay )
        {
            return;
        }

        binaryReader.BaseStream.Seek( 0, SeekOrigin.Begin );

        if( !ghost.gameObject.activeSelf )
        {
            ghost.gameObject.SetActive( true );
        }
        
        isReplaying = true;
    }

    public void StopReplaying()
    {
        isReplaying = false;

        ghost.gameObject.SetActive( false );
    }

    //----------------------------------------------------------------------------------------------------
    
    const float maxDuration = 120f;
    
    // pos XYZ - 12 bytes
    // rot XYZ - 12 bytes
    // rpm - 4 bytes
    const int stateLength = 12 + 12 + 4;
    
    BinaryWriter binaryWriter;
    BinaryReader binaryReader;
    bool isRecording;
    bool isReplaying;
    float recordingTime;
    string replayFilePath;
    bool hasReplay;
    

    void Awake()
    {
        if( ghost.gameObject.activeSelf )
        {
            ghost.gameObject.SetActive( false );
        }

        replayFilePath = Application.dataPath + "/GhostReplay.dat";
    }

    void OnDisable()
    {
        binaryWriter?.Dispose();
        binaryReader?.Dispose();
    }

    void FixedUpdate()
    {
        if( isRecording )
        {
            SaveState();
            
            recordingTime += Time.fixedDeltaTime;
            if( recordingTime >= maxDuration )
            {
                StopRecording();
            }
        }

        if( isReplaying )
        {
            var endReeached = !RestoreState();
            if( endReeached )
            {
                StopReplaying();
            }
        }
    }
    

    void SaveState()
    {
        var craftPosition = craft.Transform.position;
        var craftRotation = craft.Transform.eulerAngles;
        var motorRpm = (short)craft.Motor.rpm;
        
        binaryWriter.Write( craftPosition.x );
        binaryWriter.Write( craftPosition.y );
        binaryWriter.Write( craftPosition.z );
        
        binaryWriter.Write( craftRotation.x );
        binaryWriter.Write( craftRotation.y );
        binaryWriter.Write( craftRotation.z );

        binaryWriter.Write( motorRpm );
    }

    bool RestoreState()
    {
        if( binaryReader.BaseStream.Position + stateLength > binaryReader.BaseStream.Length )
        {
            return false;
        }

        var craftPosition = new Vector3
        {
            x = binaryReader.ReadSingle(),
            y = binaryReader.ReadSingle(),
            z = binaryReader.ReadSingle()
        };

        var craftRotation = new Vector3
        {
            x = binaryReader.ReadSingle(),
            y = binaryReader.ReadSingle(),
            z = binaryReader.ReadSingle()
        };

        var motorRpm = binaryReader.ReadInt16();

        ghost.SetState( craftPosition, craftRotation, motorRpm );

        return true;
    }

    
    async void SaveToFileAsync( byte[] data, string path )
    {
        using( var fileStream = new FileStream( path, FileMode.Create, FileAccess.Write ) )
        {
            await fileStream.WriteAsync( data, 0, data.Length );
        }
    }

    async Task<byte[]> LoadFromFileAsync( string path )
    {
        using( var fileStream = new FileStream( path, FileMode.Open, FileAccess.Read ) )
        {
            var result = new byte[ fileStream.Length ];
            await fileStream.ReadAsync( result, 0, (int)fileStream.Length );
            return result;
        }
    }
}