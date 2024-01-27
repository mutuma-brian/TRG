using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public enum ControlMode { simple = 1, touch = 2 }


public class BuggyControl : MonoBehaviour
{


    public ControlMode controlMode = ControlMode.simple;

    public bool activeControl = false;

    public TMP_Text scoreTextMeshPro;

    public string collisionTag = "RoadCollider";

    public int score = 100;


    // Wheels Setting /////////////////////////////////

    public CarWheels carWheels;

    [System.Serializable]
    public class CarWheels
    {
        public ConnectWheel wheels;
        public WheelSetting setting;
    }


    [System.Serializable]
    public class ConnectWheel
    {
        public bool frontWheelDrive = true;

        public Transform frontRight;
        public Transform frontLeft;

        public Transform frontRightAxle;
        public Transform frontLeftAxle;



        public bool backWheelDrive = true;
        public Transform backRight;
        public Transform backLeft;


        public Transform backRightAxle;
        public Transform backLeftAxle;

    }

    [System.Serializable]
    public class WheelSetting
    {
        public float FrontWheelsRadius = 0.4f;
        public float BackWheelsRadius = 0.4f;
        public float Weight = 1000.0f;
        public float Distance = 0.2f;
    }


    // Lights Setting ////////////////////////////////

    public CarLights carLights;

    [System.Serializable]
    public class CarLights
    {
        public Light[] brakeLights;
        public Light[] reverseLights;
    }

    // Car sounds /////////////////////////////////

    public CarSounds carSounds;

    [System.Serializable]
    public class CarSounds
    {
        public AudioSource IdleEngine, LowEngine, HighEngine;

        public AudioSource crash;

        public AudioSource nitro;
        public AudioSource switchGear;
    }

    // Car Particle /////////////////////////////////

    public CarParticles carParticles;

    [System.Serializable]
    public class CarParticles
    {
        public GameObject brakeParticlePerfab;
        public ParticleSystem shiftParticle1, shiftParticle2;
        private GameObject[] wheelParticle = new GameObject[4];
    }

    // Car Engine Setting /////////////////////////////////

    public CarSetting carSetting;

    [System.Serializable]
    public class CarSetting
    {

        public bool showNormalGizmos = false;
        public Transform carSteer;
        public HitGround[] hitGround;

        public List<Transform> cameraSwitchView;

        public float springs = 25000.0f;
        public float dampers = 1500.0f;

        public float carPower = 120f;
        public float shiftPower = 150f;
        public float brakePower = 8000f;

        public Vector3 shiftCentre = new Vector3(0.0f, -0.8f, 0.0f);

        public float maxSteerAngle = 30.0f;

        public float shiftDownRPM = 1500.0f;
        public float shiftUpRPM = 2500.0f;
        public float idleRPM = 500.0f;

        public float stiffness = 2.0f;

        public bool automaticGear = true;

        public float[] gears = { -10f, 9f, 6f, 4.5f, 3f, 2.5f };


        public float LimitBackwardSpeed = 60.0f;
        public float LimitForwardSpeed = 220.0f;

    }

    


    [System.Serializable]
    public class HitGround
    {
       
        public string tag = "street";
        public bool grounded = false;
        public AudioClip brakeSound;
        public AudioClip groundSound;
        public Color brakeColor;
    }


    ////////////////////////////////////////////// TouchMode (Control) ////////////////////////////////////////////////////////////////////


    [HideInInspector]
    public float accelFwd = 0.0f;
    [HideInInspector]
    public float accelBack = 0.0f;
    [HideInInspector]
    public float steerAmount = 0.0f;


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [HideInInspector]
    public float speed = 0.0f;
    [HideInInspector]
    public bool brake;
    [HideInInspector]
    public float steer = 0;
    [HideInInspector]
    public float curTorque = 100f;
    [HideInInspector]
    public float powerShift = 100;
    [HideInInspector]
    public bool shift;

    [HideInInspector]
    public int currentGear = 0;
    [HideInInspector]
    public bool NeutralGear = true;
    [HideInInspector]
    public float motorRPM = 0.0f;
    [HideInInspector]
    public bool Backward = false;


    private float lastSpeed = -10.0f;
    private float accel = 0.0f;
    private float torque = 100f;

    private bool shifmotor;
    private bool shifting = false;


    private float[] efficiencyTable = { 0.6f, 0.65f, 0.7f, 0.75f, 0.8f, 0.85f, 0.9f, 1.0f, 1.0f, 0.95f, 0.80f, 0.70f, 0.60f, 0.5f, 0.45f, 0.40f, 0.36f, 0.33f, 0.30f, 0.20f, 0.10f, 0.05f };
    private float efficiencyTableStep = 250.0f;


    private float Pitch;
    private float PitchDelay;
    private float shiftTime = 0.0f;
    private float shiftDelay = 0.0f;



    private float wantedRPM = 0.0f;
    private float w_rotate;
    private float slip, slip2 = 0.0f;


    private GameObject[] Particle = new GameObject[4];
    private Vector3 steerCurAngle;
    private Rigidbody myRigidbody;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private WheelComponent[] wheels;

    private class WheelComponent
    {

        public Transform wheel;
        public Transform axle;
        public WheelCollider collider;
        public Vector3 startPos;
        public float rotation = 0.0f;
        public float rotation2 = 0.0f;
        public float maxSteer;
        public bool drive;
        public float pos_y = 0.0f;
    }


    private WheelComponent SetWheelComponent(Transform wheel, Transform axle, float maxSteer, bool drive, float pos_y)
    {


        WheelComponent result = new WheelComponent();
        GameObject wheelCol = new GameObject(wheel.name + "WheelCollider");

        wheelCol.transform.parent = transform;
        wheelCol.transform.position = wheel.position;
        wheelCol.transform.eulerAngles = transform.eulerAngles;
        pos_y = wheelCol.transform.localPosition.y;

        WheelCollider col = (WheelCollider)wheelCol.AddComponent(typeof(WheelCollider));

        result.wheel = wheel;
        result.axle = axle;
        result.collider = wheelCol.GetComponent<WheelCollider>();
        result.drive = drive;
        result.pos_y = pos_y;
        result.maxSteer = maxSteer;
        result.startPos = wheelCol.transform.localPosition;

        return result;

    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    
    void Awake()
    {

        if (carSetting.automaticGear) NeutralGear = false;

        myRigidbody = transform.GetComponent<Rigidbody>();

        wheels = new WheelComponent[4];

        wheels[0] = SetWheelComponent(carWheels.wheels.frontRight,carWheels.wheels.frontRightAxle, carSetting.maxSteerAngle, carWheels.wheels.frontWheelDrive, carWheels.wheels.frontRight.position.y);
        wheels[1] = SetWheelComponent(carWheels.wheels.frontLeft, carWheels.wheels.frontLeftAxle, carSetting.maxSteerAngle, carWheels.wheels.frontWheelDrive, carWheels.wheels.frontLeft.position.y);

        wheels[2] = SetWheelComponent(carWheels.wheels.backRight,carWheels.wheels.backRightAxle, 0, carWheels.wheels.backWheelDrive, carWheels.wheels.backRight.position.y);
        wheels[3] = SetWheelComponent(carWheels.wheels.backLeft,carWheels.wheels.backLeftAxle, 0, carWheels.wheels.backWheelDrive, carWheels.wheels.backLeft.position.y);

        if (carSetting.carSteer)
        steerCurAngle = carSetting.carSteer.localEulerAngles;

        foreach (WheelComponent w in wheels)
        {

            WheelCollider col = w.collider;
            col.suspensionDistance = carWheels.setting.Distance;
            JointSpring js = col.suspensionSpring;

            js.spring = carSetting.springs;
            js.damper = carSetting.dampers;
            col.suspensionSpring = js;


            if (w == wheels[0] || w == wheels[1])
            {
                col.radius = carWheels.setting.FrontWheelsRadius;
            }
            else
            {
                col.radius = carWheels.setting.BackWheelsRadius;
            }
        

            col.mass = carWheels.setting.Weight;

            WheelFrictionCurve fc = col.forwardFriction;

            fc.asymptoteValue = 5000.0f;
            fc.extremumSlip = 2.0f;
            fc.asymptoteSlip = 20.0f;
            fc.stiffness = carSetting.stiffness;
            col.forwardFriction = fc;
            fc = col.sidewaysFriction;
            fc.asymptoteValue = 7500.0f;
            fc.asymptoteSlip = 2.0f;
            fc.stiffness = carSetting.stiffness;
            col.sidewaysFriction = fc;

        }
    }




    public void ShiftUp()
    {
        float now = Time.timeSinceLevelLoad;

        if (now < shiftDelay) return;

        if (currentGear < carSetting.gears.Length - 1)
        {

           // if (!carSounds.switchGear.isPlaying)
                carSounds.switchGear.GetComponent<AudioSource>().Play();


                if (!carSetting.automaticGear)
            {
                if (currentGear == 0)
                {
                    if (NeutralGear){currentGear++;NeutralGear = false;}
                    else
                    { NeutralGear = true;}
                }
                else
                {
                    currentGear++;
                }
            }
            else
            {
                currentGear++;
            }


           shiftDelay = now + 1.0f;
           shiftTime = 1.5f;
        }
    }




    public void ShiftDown()
    {
        float now = Time.timeSinceLevelLoad;

       if (now < shiftDelay) return;

        if (currentGear > 0 || NeutralGear)
        {

           //w if (!carSounds.switchGear.isPlaying)
                carSounds.switchGear.GetComponent<AudioSource>().Play();

                if (!carSetting.automaticGear)
            {

                if (currentGear == 1)
                {
                    if (!NeutralGear){currentGear--;NeutralGear = true;}
                }
                else if (currentGear == 0){NeutralGear = false;}else{currentGear--;}
            }
            else
            {
                currentGear--;
            }


            shiftDelay = now + 0.1f;
            shiftTime = 2.0f;
        }
    }




    void OnCollisionEnter(Collision collision)
    {

        if (!carSounds.crash.isPlaying)
            carSounds.crash.Play();

        if (collision.transform.root.GetComponent<BuggyControl>())
        {
            collision.transform.root.GetComponent<BuggyControl>().slip2 = Mathf.Clamp(collision.relativeVelocity.magnitude, 0.0f, 5.0f);

            myRigidbody.angularVelocity = new Vector3(-myRigidbody.angularVelocity.x * 0.5f, myRigidbody.angularVelocity.y * 0.5f, -myRigidbody.angularVelocity.z * 0.5f);
            myRigidbody.velocity = new Vector3(myRigidbody.velocity.x, myRigidbody.velocity.y * 0.5f, myRigidbody.velocity.z);
        }

       if (collision.gameObject.CompareTag(collisionTag))
        {
            score = score - 5;
            scoreTextMeshPro.text = "Score : " + score.ToString();
        }


    }


    void OnCollisionStay(Collision collision)
    {

        if (collision.transform.root.GetComponent<BuggyControl>())
            collision.transform.root.GetComponent<BuggyControl>().slip2 = 5.0f;
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void Update()
    {


        if (!carSetting.automaticGear && activeControl)
        {
            if (Input.GetKeyDown("page up"))
            {
                ShiftUp();


            }
            if (Input.GetKeyDown("page down"))
            {
                ShiftDown();

            }
        }

    }



    void FixedUpdate()
    {


        // speed of car
        speed = myRigidbody.velocity.magnitude * 2.7f;

        if (speed < lastSpeed - 10 && slip < 10) slip = lastSpeed / 15;
        
        lastSpeed = speed;


        if (slip2 != 0.0f)
            slip2 = Mathf.MoveTowards(slip2, 0.0f, 0.1f);


        myRigidbody.centerOfMass = carSetting.shiftCentre;
        


        if (activeControl)
        {
            if (controlMode == ControlMode.simple)
            {

                accel = 0;
                brake = false;
                shift = false;

                if (carWheels.wheels.frontWheelDrive || carWheels.wheels.backWheelDrive)
                {
                    steer = Mathf.MoveTowards(steer, SimpleInput.GetAxis("Horizontal"), 0.2f);
                    accel = SimpleInput.GetAxis("Vertical");
                    brake = Input.GetButton("Jump");
                    shift = Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift);
                }

            }
            else if (controlMode == ControlMode.touch)
            {

                if (accelFwd != 0) { accel = accelFwd; } else { accel = accelBack; }
                steer = Mathf.MoveTowards(steer, steerAmount, 0.07f);

            }

        }
        else
        {
            accel = 0.0f;
            steer = 0.0f;
            brake = false;
            shift = false;
        }



        if (!carWheels.wheels.frontWheelDrive && !carWheels.wheels.backWheelDrive)
            accel = 0.0f;



        if (carSetting.carSteer)
            carSetting.carSteer.localEulerAngles = new Vector3(steerCurAngle.x, steerCurAngle.y, steerCurAngle.z + (steer * -120.0f));



        if (carSetting.automaticGear && (currentGear == 1) && (accel < 0.0f))
        {
            if (speed < 5.0f)
                ShiftDown();


        }
        else if (carSetting.automaticGear && (currentGear == 0) && (accel > 0.0f))
        {
            if (speed < 5.0f)
                ShiftUp();

        }
        else if (carSetting.automaticGear && (motorRPM > carSetting.shiftUpRPM) && (accel > 0.0f) && speed > 10.0f && !brake)
        {

            ShiftUp();

        }
        else if (carSetting.automaticGear && (motorRPM < carSetting.shiftDownRPM) && (currentGear > 1))
        {
            ShiftDown();
        }



        if (speed < 1.0f) Backward = true;



        if (currentGear == 0 && Backward == true)
        {
          //  carSetting.shiftCentre.z = -accel / -5;
            if (speed < carSetting.gears[0] * -10)
                accel = -accel;
        }
        else
        {
            Backward = false;
         //   if (currentGear > 0)
         //   carSetting.shiftCentre.z = -(accel / currentGear) / -5;
        }

      //  carSetting.shiftCentre.x = -Mathf.Clamp(steer * (speed / 100), -0.03f, 0.03f);

        // Brake Lights

        foreach (Light brakeLight in carLights.brakeLights)
        {
            if (brake || accel < 0 || speed < 1.0f)
            {
                brakeLight.intensity = Mathf.MoveTowards(brakeLight.intensity, 8, 0.5f);
            }
            else
            {
                brakeLight.intensity = Mathf.MoveTowards(brakeLight.intensity, 0, 0.5f);

            }

            brakeLight.enabled = brakeLight.intensity == 0 ? false : true;
        }


        // Reverse Lights

        foreach (Light WLight in carLights.reverseLights)
        {
            if (speed > 2.0f && currentGear == 0)
            {
                WLight.intensity = Mathf.MoveTowards(WLight.intensity, 8, 0.5f);
            }
            else
            {
                WLight.intensity = Mathf.MoveTowards(WLight.intensity, 0, 0.5f);
            }
            WLight.enabled = WLight.intensity == 0 ? false : true;
        }




        wantedRPM = (5500.0f * accel) * 0.1f + wantedRPM * 0.9f;

        float rpm = 0.0f;
        int motorizedWheels = 0;
        bool floorContact = false;
        int currentWheel = 0;

        foreach (WheelComponent w in wheels)
        {
            WheelHit hit;
            WheelCollider col = w.collider;

            if (w.drive)
            {
                if (!NeutralGear && brake && currentGear < 2)
                {
                    rpm += accel * carSetting.idleRPM;

                    /*
                    if (rpm > 1)
                    {
                        carSetting.shiftCentre.z = Mathf.PingPong(Time.time * (accel * 10), 2.0f) - 1.0f;
                    }
                    else
                    {
                        carSetting.shiftCentre.z = 0.0f;
                    }
                    */

                }
                else
                {
                    if (!NeutralGear)
                    {
                        rpm += col.rpm;
                    }else{
                        rpm += (carSetting.idleRPM*accel);
                    }
                }


                motorizedWheels++;
            }




            if (brake || accel < 0.0f)
            {

                if ((accel < 0.0f) || (brake && (w == wheels[2] || w == wheels[3])))
                {

                    if (brake && (accel > 0.0f))
                    {
                        slip = Mathf.Lerp(slip, 5.0f, accel * 0.01f);
                    }
                    else if (speed > 1.0f)
                    {
                        slip = Mathf.Lerp(slip, 1.0f, 0.002f);
                    }
                    else
                    {
                        slip = Mathf.Lerp(slip, 1.0f, 0.02f);
                    }


                    wantedRPM = 0.0f;
                    col.brakeTorque = carSetting.brakePower;
                    w.rotation = w_rotate;

                }
            }
            else
            {


                col.brakeTorque = accel == 0 || NeutralGear ? col.brakeTorque = 1000 : col.brakeTorque = 0;


                slip = speed > 0.0f ?
    (speed > 100 ? slip = Mathf.Lerp(slip, 1.0f + Mathf.Abs(steer), 0.02f) : slip = Mathf.Lerp(slip, 1.5f, 0.02f))
    : slip = Mathf.Lerp(slip, 0.01f, 0.02f);


                w_rotate = w.rotation;

            }


            WheelFrictionCurve fc = col.forwardFriction;

            fc.asymptoteValue = 5000.0f;
            fc.extremumSlip = 2.0f;
            fc.asymptoteSlip = 20.0f;
            fc.stiffness = carSetting.stiffness / (slip + slip2);
            col.forwardFriction = fc;
            fc = col.sidewaysFriction;
            fc.stiffness = carSetting.stiffness / (slip + slip2);


            fc.extremumSlip = 0.3f + Mathf.Abs(steer);

            col.sidewaysFriction = fc;




            if (shift && (currentGear > 1 && speed > 50.0f) && shifmotor && Mathf.Abs(steer) < 0.2f)
            {

                if (powerShift == 0) { shifmotor = false; }

                powerShift = Mathf.MoveTowards(powerShift, 0.0f, Time.deltaTime * 10.0f);

                carSounds.nitro.volume = Mathf.Lerp(carSounds.nitro.volume, 1.0f, Time.deltaTime * 10.0f);

                if (!carSounds.nitro.isPlaying)
                {
                    carSounds.nitro.GetComponent<AudioSource>().Play();

                }


                curTorque = powerShift > 0 ? carSetting.shiftPower : carSetting.carPower;
                carParticles.shiftParticle1.emissionRate = Mathf.Lerp(carParticles.shiftParticle1.emissionRate, powerShift > 0 ? 50 : 0, Time.deltaTime * 10.0f);
                carParticles.shiftParticle2.emissionRate = Mathf.Lerp(carParticles.shiftParticle2.emissionRate, powerShift > 0 ? 50 : 0, Time.deltaTime * 10.0f);
            }
            else
            {

                if (powerShift > 20)
                {
                    shifmotor = true;
                }

                carSounds.nitro.volume = Mathf.MoveTowards(carSounds.nitro.volume, 0.0f, Time.deltaTime * 2.0f);

                if (carSounds.nitro.volume == 0)
                    carSounds.nitro.Stop();

                powerShift = Mathf.MoveTowards(powerShift, 100.0f, Time.deltaTime * 5.0f);
                curTorque = carSetting.carPower;
                carParticles.shiftParticle1.emissionRate = Mathf.Lerp(carParticles.shiftParticle1.emissionRate, 0, Time.deltaTime * 10.0f);
                carParticles.shiftParticle2.emissionRate = Mathf.Lerp(carParticles.shiftParticle2.emissionRate, 0, Time.deltaTime * 10.0f);
            }


            w.rotation = Mathf.Repeat(w.rotation + Time.deltaTime * col.rpm * 360.0f / 60.0f, 360.0f);
            w.rotation2 = Mathf.Lerp(w.rotation2,col.steerAngle,0.1f);
            w.wheel.localRotation = Quaternion.Euler(w.rotation,w.rotation2, 0.0f);



            Vector3 lp = w.axle.localPosition;


            if (col.GetGroundHit(out hit))
            {


                if (carParticles.brakeParticlePerfab)
                {
                    if (Particle[currentWheel] == null)
                    {
                        Particle[currentWheel] = Instantiate(carParticles.brakeParticlePerfab, w.wheel.position-(Vector3.up*(w.collider.radius-0.5f)), Quaternion.identity) as GameObject;
                        Particle[currentWheel].name = "WheelParticle";
                        Particle[currentWheel].transform.parent = transform;
                        Particle[currentWheel].AddComponent<AudioSource>();
                        Particle[currentWheel].GetComponent<AudioSource>().maxDistance = 50;
                        Particle[currentWheel].GetComponent<AudioSource>().spatialBlend = 1;
                        Particle[currentWheel].GetComponent<AudioSource>().dopplerLevel = 5;
                        Particle[currentWheel].GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Custom;
                    }


                    var pc = Particle[currentWheel].GetComponent<ParticleSystem>();
                    bool WGrounded = false;


                    for (int i = 0; i < carSetting.hitGround.Length; i++)
                    {

                        if (hit.collider.CompareTag(carSetting.hitGround[i].tag))
                        {
                            WGrounded = carSetting.hitGround[i].grounded;

                            if ((brake || Mathf.Abs(hit.sidewaysSlip) > 0.5f) && speed > 1)
                            {
                                Particle[currentWheel].GetComponent<AudioSource>().clip = carSetting.hitGround[i].brakeSound;
                            }
                            else if (Particle[currentWheel].GetComponent<AudioSource>().clip != carSetting.hitGround[i].groundSound && !Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                            {

                                Particle[currentWheel].GetComponent<AudioSource>().clip = carSetting.hitGround[i].groundSound;
                            }

                            Particle[currentWheel].GetComponent<ParticleSystem>().startColor = carSetting.hitGround[i].brakeColor;

                        }


                    }


                    if (WGrounded && speed > 5 && !brake)
                    {

                        pc.enableEmission = true;

                        Particle[currentWheel].GetComponent<AudioSource>().volume = 0.5f;

                        if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                            Particle[currentWheel].GetComponent<AudioSource>().Play();

                    }
                    else if ((brake || Mathf.Abs(hit.sidewaysSlip) > 0.6f) && speed > 1)
                    {

                        if ((accel < 0.0f) || ((brake || Mathf.Abs(hit.sidewaysSlip) > 0.6f) && (w == wheels[2] || w == wheels[3])))
                        {

                            if (!Particle[currentWheel].GetComponent<AudioSource>().isPlaying)
                                Particle[currentWheel].GetComponent<AudioSource>().Play();
                            pc.enableEmission = true;
                            Particle[currentWheel].GetComponent<AudioSource>().volume = 10;

                        }

                    }
                    else
                    {

                        pc.enableEmission = false;
                        Particle[currentWheel].GetComponent<AudioSource>().volume = Mathf.Lerp(Particle[currentWheel].GetComponent<AudioSource>().volume, 0, Time.deltaTime * 10.0f);
                    }

                }


                lp.y -= Vector3.Dot(w.axle.position - hit.point, Vector3.up / transform.lossyScale.x) - (col.radius);
                lp.y = Mathf.Clamp(lp.y, -10.0f, w.pos_y);
                floorContact = floorContact || (w.drive);


            }
            else
            {

                if (Particle[currentWheel] != null)
                {
                    var pc = Particle[currentWheel].GetComponent<ParticleSystem>();
                    pc.enableEmission = false;
                }



                lp.y = w.startPos.y - carWheels.setting.Distance;

                myRigidbody.AddForce(Vector3.down * 5000);

            }

            currentWheel++;
            w.axle.localPosition = new Vector3(w.axle.localPosition.x, lp.y, w.axle.localPosition.z);


        }

        if (motorizedWheels > 1)
        {
            rpm = rpm / motorizedWheels;
        }


        motorRPM = 0.95f * motorRPM + 0.05f * Mathf.Abs(rpm * carSetting.gears[currentGear]);
        if (motorRPM > 5500.0f) motorRPM = 5200.0f;


        int index = (int)(motorRPM / efficiencyTableStep);
        if (index >= efficiencyTable.Length) index = efficiencyTable.Length - 1;
        if (index < 0) index = 0;



        float newTorque = curTorque * carSetting.gears[currentGear] * efficiencyTable[index];

        foreach (WheelComponent w in wheels)
        {
            WheelCollider col = w.collider;

            if (w.drive)
            {

                if (Mathf.Abs(col.rpm) > Mathf.Abs(wantedRPM))
                {

                    col.motorTorque = 0;
                }
                else
                {
                    // 
                    float curTorqueCol = col.motorTorque;

                    if (!brake && accel != 0 && NeutralGear == false)
                    {
                        if ((speed < carSetting.LimitForwardSpeed && currentGear > 0) ||
                            (speed < carSetting.LimitBackwardSpeed && currentGear == 0))
                        {

                            col.motorTorque = curTorqueCol * 0.9f + newTorque * 1.0f;
                        }
                        else
                        {
                            col.motorTorque = 0;
                            col.brakeTorque = 2000;
                        }


                    }
                    else
                    {
                        col.motorTorque = 0;

                    }
                }

            }





            if (brake || slip2 > 2.0f)
            {
                col.steerAngle = Mathf.Lerp(col.steerAngle, steer * w.maxSteer, 0.02f);
            }
            else
            {

                float SteerAngle = Mathf.Clamp(speed / carSetting.maxSteerAngle, 1.0f, carSetting.maxSteerAngle);
                col.steerAngle = steer * (w.maxSteer / SteerAngle);


            }

        }




        // calculate pitch (keep it within reasonable bounds)
        Pitch = Mathf.Clamp(1.2f + ((motorRPM - carSetting.idleRPM) / (carSetting.shiftUpRPM - carSetting.idleRPM)), 1.0f, 10.0f);

        shiftTime = Mathf.MoveTowards(shiftTime, 0.0f, 0.1f);

        if (Pitch == 1)
        {
            carSounds.IdleEngine.volume = Mathf.Lerp(carSounds.IdleEngine.volume, 1.0f, 0.1f);
            carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.5f, 0.1f);
            carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 0.0f, 0.1f);

        }
        else
        {

            carSounds.IdleEngine.volume = Mathf.Lerp(carSounds.IdleEngine.volume, 1.8f - Pitch, 0.1f);


            if ((Pitch > PitchDelay || accel > 0) && shiftTime == 0.0f)
            {
                carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.0f, 0.2f);
                carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 1.0f, 0.1f);
            }
            else
            {
                carSounds.LowEngine.volume = Mathf.Lerp(carSounds.LowEngine.volume, 0.5f, 0.1f);
                carSounds.HighEngine.volume = Mathf.Lerp(carSounds.HighEngine.volume, 0.0f, 0.2f);
            }




            carSounds.HighEngine.pitch = Pitch;
            carSounds.LowEngine.pitch = Pitch;

            PitchDelay = Pitch;
        }

    }




    /////////////// Show Normal Gizmos ////////////////////////////

    void OnDrawGizmos()
    {

        if (!carSetting.showNormalGizmos || Application.isPlaying) return;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);

        Gizmos.matrix = rotationMatrix;
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawCube(Vector3.up/1.5f, new Vector3(2.5f, 2.0f, 6));
        Gizmos.DrawSphere(carSetting.shiftCentre / transform.lossyScale.x, 0.2f);

    }




}