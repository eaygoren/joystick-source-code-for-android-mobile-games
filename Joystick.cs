using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Joystick : MonoBehaviour
{
    public Transform pirate, ship, circle, point;       //pirate; for movement, ship; for rotate, circle; joystick area, point; for joystick movement
    public Vector2 pointA, pointB, offset, magnitude;
    public Vector3 target;
    public Touch leftTouch, rightTouch;

    public float speed, degree, angle;
    public bool touching;

    private void FixedUpdate()
    {
        TouchControl();
        Touching();
        Calculate();
    }

    public void TouchControl()
    {
        for (int i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).position.x < Screen.width / 2)
            {
                leftTouch = Input.GetTouch(i);
            }
            else
            if (Input.GetTouch(i).position.x > Screen.width / 2)
            {
                rightTouch = Input.GetTouch(i);
            }
        }
    }

    public void Touching()
    {
        switch (leftTouch.phase)
        {
            case TouchPhase.Began:
                pointA = leftTouch.position;
                circle.transform.position = pointA;
                break;

            case TouchPhase.Moved:
                pointB = leftTouch.position;
                point.transform.position = pointB;

                circle.GetComponent<Image>().enabled = true;
                point.GetComponent<Image>().enabled = true;

                touching = true;
                break;
                
            case TouchPhase.Ended:
                touching = false;
                break;
        }
    }

    public void Calculate()
    {
        if (touching)
        {
            offset = pointB - pointA;
            magnitude = Vector2.ClampMagnitude(offset, 30F);
            Movement(magnitude);

            point.transform.position = new Vector2((pointA.x + magnitude.x), (pointA.y + magnitude.y));
            
            target = point.transform.position - circle.transform.position;
            angle = Vector3.Angle(target, pirate.transform.up);

            if (point.transform.position.x < circle.transform.position.x)
            {
                angle = 360 - angle;
            }
            Rotation(angle);
        }
        else
        {
            point.GetComponent<Image>().enabled = false;
            circle.GetComponent<Image>().enabled = false;
        }
    }

    public void Movement(Vector2 direction)
    {
        pirate.Translate(direction * speed * Time.deltaTime);
    }

    public void Rotation(float degree)
    {
        ship.transform.eulerAngles = new Vector3(0, 0, (180 - degree));
    }
}