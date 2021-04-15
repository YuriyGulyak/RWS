// https://gist.github.com/eelstork/073cfd6eb43fe2bdcbf3

using UnityEngine;

namespace RWS
{
    public class Orientation : MonoBehaviour
    {
        Vector3 pitchYawRoll
        {
            get { return new Vector3( pitch, yaw, roll ); }
            set
            {
                transform.rotation = Quaternion.identity;
                Vector3 o = transform.position;
                transform.RotateAround( o, Vector3.forward, value.z );
                transform.RotateAround( o, Vector3.right, value.x );
                transform.RotateAround( o, Vector3.up, value.y );
            }
        }


        // Pitch indicates whether a vehicle is pointing up or down.
        // It is the angle between the forward vector and the horizontal plane.
        public float pitch
        {
            get
            {
                var sine = UnaryTrim( transform.forward.y );
                return -Mathf.Asin( sine ) * Mathf.Rad2Deg;
            }
            set { pitchYawRoll = new Vector3( value, yaw, roll ); }
        }

        // Yaw is the angle between the forward vector's ground image and the forward/north direction.
        // Yaw is the general 'direction' or 'course'.
        // if a vehicle is pointing all the way up or down, we extract yaw from the right vector.
        public float yaw
        {
            get
            {
                var vector = Ground( transform.forward );
                if( vector.magnitude < 0.5f )
                {
                    return EvalAltYaw();
                }

                var alpha = Vector3.Angle( vector, Vector3.forward );
                return vector.x > 0f ? alpha : -alpha;
            }
            set { pitchYawRoll = new Vector3( pitch, value, roll ); }
        }

        // Roll is the angle between the right vector and its ground image.
        public float roll
        {
            get
            {
                float sine = UnaryTrim( transform.right.y );
                return Mathf.Asin( sine ) * Mathf.Rad2Deg;
            }
            set { pitchYawRoll = new Vector3( pitch, yaw, value ); }
        }


        public float attitude
        {
            get { return pitch; }
            set { pitch = value; }
        }

        public float heading
        {
            get { return yaw; }
            set { yaw = value; }
        }

        public float bank
        {
            get { return roll; }
            set { roll = value; }
        }


        // PRIVATE ---------------------------------------------------------


        // Alternatively Yaw can be calculated as the angle between the ground projection of the right vector and the east/right direction.
        // Even if a vehicle is pointing straight up or dawn, you can still tell it's course/direction looking at position of the wings/tyres.
        float EvalAltYaw()
        {
            var vector = Ground( transform.right );
            var alpha = Vector3.Angle( vector, Vector3.right );
            return vector.z < 0 ? alpha : -alpha;
        }

        static Vector3 Ground( Vector3 u )
        {
            u.y = 0f;
            return u;
        }

        static float UnaryTrim( float w )
        {
            if( w > 1.0 ) return 1.0f;
            if( w < -1.0 ) return -1.0f;
            return w;
        }


        //void Update()
        //{
        //    print( $"{roll} | {pitch} | {yaw} | " );
        //}
    }
}