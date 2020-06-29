using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileplatformDef : MonoBehaviour
{
    private float angle = 0;
    private Square mySquare;
    private Triangle myTriangle;
    private Points myPoints;
    private int sentido = 1;
    private int pos = 0;

    private float initX = 0f;
    private float initY = 0f;
    //[SerializeField]
    private float currentTime = 0f;
    private float sentidoDeLinea = 0.05f;

    public MpComponents components;
    public MpProperties properties;
    // Start is called before the first frame update
    void Start()
    {
        if (properties.movementType == MpProperties.MovementType.Square) 
        {
            mySquare = new Square(components.rigidbody2D, properties.squareWidth, properties.squareHeight);
        }
        if (properties.movementType == MpProperties.MovementType.Tringle)
        {
            myTriangle = new Triangle(components.rigidbody2D, properties.triangleWidth, properties.triangleHeight);
        }
        /*if (properties.movementType == MpProperties.MovementType.MultipleLines)
        {
            myPoints = new Points();
            myPoints.X1 = properties.points[pos].Item1;
            myPoints.Y1 = properties.points[pos].Item2;
            myPoints.X2 = properties.points[pos + 1].Item1;
            myPoints.Y2 = properties.points[pos + 1].Item2;
        }*/
        initX = components.rigidbody2D.position.x;
        initY = components.rigidbody2D.position.y;
        angle = properties.initialAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (properties.movementType == MpProperties.MovementType.Line) { vaiven(); }
    }

    private void FixedUpdate()
    {
        if (properties.movementType == MpProperties.MovementType.Circumference) { circle(); }
        if (properties.movementType == MpProperties.MovementType.Square) { square(); }
        if (properties.movementType == MpProperties.MovementType.Tringle) { triangle(); }
        //if (properties.movementType == MpProperties.MovementType.MultipleLines) { points(); }
        //if (properties.movementType == MpProperties.MovementType.Wave) { wave(); }
    }

    private void vaiven()
    {
        // pos = posinit + vel*t
        components.rigidbody2D.position = new Vector2(initX + properties.speedX * currentTime, initY + properties.speedY * currentTime);
        currentTime += sentidoDeLinea;

        if (currentTime >= properties.lineTime)
        {
            currentTime = properties.lineTime;
            sentidoDeLinea = -0.05f;
        }
        if (currentTime <= 0)
        {
            currentTime = 0;
            sentidoDeLinea = 0.05f;
        }
    }

    private void circle()
    {
        float radians = angle * (Mathf.PI / 180);
        float deltaX = Mathf.Cos(radians);
        float deltaY = Mathf.Sin(radians);

        float currentX = components.rigidbody2D.position.x;
        float currentY = components.rigidbody2D.position.y;

        components.rigidbody2D.position = new Vector2(currentX + (deltaX * properties.circleRadius), currentY + (deltaY * properties.circleRadius));

        angle += properties.speed+properties.velocityCircum;
        if (angle > 360)
        {
            angle = 0;
        }
    }

    private void square()
    {
        if(mySquare.isIn == "A")
        {
            float currenX = components.rigidbody2D.position.x + properties.speed;
            components.rigidbody2D.position = new Vector2(currenX, mySquare.squareAy);
            if (currenX >= mySquare.squareBx) { components.rigidbody2D.position = new Vector2(mySquare.squareBx, mySquare.squareBy); mySquare.isIn = "B"; }
        }
        if (mySquare.isIn == "B")
        {
            float currenY = components.rigidbody2D.position.y + properties.speed;
            components.rigidbody2D.position = new Vector2(mySquare.squareBx, currenY);
            if (currenY >= mySquare.squareCy) { components.rigidbody2D.position = new Vector2(mySquare.squareCx, mySquare.squareCy); mySquare.isIn = "C"; }
        }
        if (mySquare.isIn == "C")
        {
            float currenX = components.rigidbody2D.position.x - properties.speed;
            components.rigidbody2D.position = new Vector2(currenX, mySquare.squareCy);
            if (currenX <= mySquare.squareDx) { components.rigidbody2D.position = new Vector2(mySquare.squareDx, mySquare.squareDy); mySquare.isIn = "D"; }
        }
        if (mySquare.isIn == "D")
        {
            float currenY = components.rigidbody2D.position.y - properties.speed;
            components.rigidbody2D.position = new Vector2(mySquare.squareDx, currenY);
            if (currenY <= mySquare.squareAy) { components.rigidbody2D.position = new Vector2(mySquare.squareAx, mySquare.squareAy); mySquare.isIn = "A"; }
        }


    }


    private void triangle()
    {
        if (myTriangle.isIn == "A")
        {
            float currenX = components.rigidbody2D.position.x + properties.speed;
            components.rigidbody2D.position = new Vector2(currenX, myTriangle.triangleAy);
            if (currenX >= myTriangle.triangleBx) { components.rigidbody2D.position = new Vector2(myTriangle.triangleBx, myTriangle.triangleBy); myTriangle.isIn = "B"; }
        }
        if (myTriangle.isIn == "B")
        {
            float currenY = components.rigidbody2D.position.y + properties.speed;
            components.rigidbody2D.position = new Vector2(myTriangle.triangleBx, currenY);
            if (currenY >= myTriangle.triangleCy) { components.rigidbody2D.position = new Vector2(myTriangle.triangleCx, myTriangle.triangleCy); myTriangle.isIn = "C"; }
        }
        if (myTriangle.isIn == "C")
        {
            float pendiente = (myTriangle.triangleAy - myTriangle.triangleCy) / (myTriangle.triangleAx - myTriangle.triangleCx);
            float currenX = components.rigidbody2D.position.x - properties.speed;
            float currentY = pendiente * (currenX - myTriangle.triangleAx) + myTriangle.triangleAy;
            components.rigidbody2D.position = new Vector2(currenX, currentY);
            if (currenX <= myTriangle.triangleAx && currentY <= myTriangle.triangleAy) 
            { 
                components.rigidbody2D.position = new Vector2(myTriangle.triangleAx, myTriangle.triangleAy); 
                myTriangle.isIn = "A"; 
            }
        }
    }


    private void points()
    {
        if (properties.points.Length > 1)
        {
                //Pendiente de la recta
            float m = (myPoints.Y2 - myPoints.Y1) / (myPoints.X2 - myPoints.X1);

                //actual x
            float x = myPoints.X1;
                //actual y, dado ecuación de la recta (punto pendiente)
            float y = m * (x - myPoints.X1) + myPoints.Y1;

            if (x < myPoints.X2)
            {
                components.rigidbody2D.position = new Vector2(x, y);
                x += properties.speed;
                y = m * (x - myPoints.X1) + myPoints.Y1;
            }
            if (x >= myPoints.X2)//Si llega al punto, se actualiza la direccion
            {
                components.rigidbody2D.position = new Vector2(myPoints.X2, myPoints.Y2);
                if (pos <= properties.points.Length-2)
                {
                    pos += sentido;
                    myPoints.currentPoints(properties.points[pos].Item1, properties.points[pos].Item2, properties.points[pos + sentido].Item1, properties.points[pos + sentido].Item2);
                }
                if (pos == properties.points.Length-1)
                {
                    sentido = -sentido;
                }
                
            }
        }
        
    }


    private void wave()
    {
        //y(x, t) = A sen(kx - wt)
        properties.currentX = components.rigidbody2D.position.x + properties.speed;
        properties.currentTime = properties.currentTime + properties.timeScale;
        float x = properties.currentX;
        float k = 2 * Mathf.PI / properties.longitudOnda;
        float w = 2 * Mathf.PI * properties.frequency*properties.waveDirect;
        float t = properties.currentTime;
        float A = properties.amplitud;

        float y = A * Mathf.Sin(k * x - w * t);

        components.rigidbody2D.position = new Vector2(x, y);

        if (t>= properties.waveTime)
        {
            properties.currentTime = 0;
            properties.waveDirect = -Mathf.Abs(properties.waveDirect);
        }
        if (components.rigidbody2D.position.x < initX)
        {
            components.rigidbody2D.position = new Vector2(initX, initY);
        }
    }

}


[System.Serializable]
public class MpComponents
{
    public Transform transform;
    public Rigidbody2D rigidbody2D;

}

[System.Serializable]
public class MpProperties
{

    public enum MovementType { Line, Tringle, Square, Circumference }//, MultipleLines, Wave}
    [Header("Movement patron")]
    public MovementType movementType=MovementType.Line;
    [Range(0, 3)]
    public float speed = 1f;
    [Range(0, 360)]
    public float initialAngle = 0f;

    [Header("Line settings")]
    [Range(0, 10)]
    public float speedX = 0f;
    [Range(0, 10)]
    public float speedY = 0f;
    [Range(0, 50)]
    public float lineTime = 0f;

    [Header("Triangle settings")]
    [Range(0, 100)]
    public float triangleWidth = 0;
    [Range(0, 100)]
    public float triangleHeight = 0;

    [Header("Square settings")]
    [Range(0, 100)]
    public float squareWidth = 0;
    [Range(0, 100)]
    public float squareHeight = 0;

    [Header("Circumference settings")]
    [Range(-5,5)]
    public float circleRadius = 1;
    [Range(-5, 5)]
    public float velocityCircum = 1;


    [Header("MultipleLines settings")]
    [SerializeField]
    public (float , float )[] points;

    [Header("Wave settings")]
    [Range(0, 100)]
    public float amplitud = 1;
    [Range(0, 100)]
    public float longitudOnda = 1;
    [Range(0, 100)]
    public float waveTime = 1;
    [Range(0,5)]
    public float frequency = 1;
    [Range(0, 5)]
    public float timeScale = 0.05f;
    [HideInInspector]
    public float currentX = 0f;
    //[HideInInspector]
    public float currentTime = 0f;
    [HideInInspector]
    public float waveDirect = 1f;

}

[System.Serializable]
public class Square
{
    public float squareAx = 0;
    public float squareAy = 0;
    public float squareBx = 0;
    public float squareBy = 0;
    public float squareCx = 0;
    public float squareCy = 0;
    public float squareDx = 0;
    public float squareDy = 0;
    public string isIn = "A";
    public Square(Rigidbody2D square, float width, float height){
        squareAx = square.position.x;
        squareAy = square.position.y;

        squareBx = squareAx + width;
        squareBy = squareAy;

        squareCx = squareBx;
        squareCy = squareBy + height;

        squareDx = squareCx - width;
        squareDy = squareCy;
    }
}


[System.Serializable]
public class Triangle
{
    public float triangleAx = 0;
    public float triangleAy = 0;
    public float triangleBx = 0;
    public float triangleBy = 0;
    public float triangleCx = 0;
    public float triangleCy = 0;
    public string isIn = "A";
    public Triangle(Rigidbody2D square, float width, float height)
    {
        triangleAx = square.position.x;
        triangleAy = square.position.y;

        triangleBx = triangleAx + width;
        triangleBy = triangleAy;

        triangleCx = triangleBx;
        triangleCy = triangleBy + height;
    }
}


[System.Serializable]
public class Points
{
    public float X1 = 0;
    public float Y1 = 0;
    public float X2 = 0;
    public float Y2 = 0;

    public void currentPoints(float x1, float y1, float x2, float y2)
    {
        X1 = x1;
        Y1 = y1;
        X2 = x2;
        Y2 = y2;
    }


}