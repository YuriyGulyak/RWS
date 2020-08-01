using System;
using UnityEngine;

[Serializable]
public class MotorModel
{
    // CONFIG
    
    public double Kv = 2500d;     // Velocity constant, RPM/V
    public double R = 0.075d;     // Armature resistance, Ohm
    public double L = 0.001d;     // Armature inductance, H
    public double J = 0.001d;     // Load and armature inertia, Kg*m^2
    public double B = 0.0001d;    // Damping, Nm/(rad/s)
    //public double Fs = 0.0001d;   // Static friction, Nm/(rad/s)
    public double Poles = 14d;    // Poles / 2 = Number of pole pairs
    
    // INPUT
    
    public double Vin = 16d;      // Armature voltage
    public double Tl = 0d;        // Load torque, Nm
    
    // OUTPUT
    
    public double I;              // Armature current, A
    public double Omega;          // Rotor angular speed, rad/s
    public double RPM;            // Revolutions per minute
    
    //----------------------------------------------------------------------------------------------------
    
    public void Init()
    {
        Kt = 1d / ( Kv / 30d * Math.PI );
    }
    
    public void Step( double dt )
    {
        Ve = Omega * Kt;
        Vl = ( Vin - Ve ) - ( I * R );
        
        I += ( Vl / L ) * dt;
        
        Te = ( I * Kt );
        Tm = Te * ( Poles / 2f ) - Tl - Omega * B;
        
        /*
        if( Tm > 0f && Tm <= Fs )
        {
            Tm = 0f;
        }
        else if( Tm >= Fs )
        {
            Tm -= Fs;
        }
        else if( Tm < 0f && Tm >= -Fs )
        {
            Tm = 0f;
        }
        else if( Tm <= -Fs )
        {
            Tm += Fs;
        }
        */
        
        Omega += Tm / J * dt;
        RPM = Omega / Mathf.PI * 30f;
    }
    
    //----------------------------------------------------------------------------------------------------
    
    // PRIVATE
    
    double Kt;    // Torque constant, Nm/A == V/(rad/s)
    double Ve;    // Back EMF, V
    double Vl;    // 
    double Te;    // Electrical torque, Nm
    double Tm;    // Mechanical torque, Nm
}
