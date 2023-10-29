
using UnityEngine;

public class CastExtension 
{
    public static bool ArcCast(Vector3 center, Quaternion rotation, float angle, float radius, int precision, int sign, out RaycastHit hit)
    {
        rotation *= Quaternion.Euler(-angle / 2, 0, 0);
        Vector3 forwardRadius = sign*radius * Vector3.forward ;
        Debug.Log(forwardRadius);
        float dAngle = angle / precision;
        Vector3 A, B, AB;
        A = forwardRadius;
        B = Quaternion.Euler(dAngle, 0, 0) * forwardRadius;
        AB = B - A;
        float AB_magnitude = AB.magnitude * 1.001f;

        for (int i = 0; i < precision; i++)
        {
            A = center + rotation * forwardRadius;
            rotation *= Quaternion.Euler(dAngle, 0, 0);
            B = center + rotation * forwardRadius;
            AB = B - A;
            Debug.DrawRay(A, AB, UnityEngine.Color.red);
            if (Physics.Raycast(A, AB, out hit, AB_magnitude))
            {
                return true;
            }
        }
        hit = new RaycastHit();
        return false;
    }
}
