using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tyre
{
    static public Player inst;




    public float loadJumpTime = 0.5f;
    public AnimationCurve loadJumpCurve;
    public float jumpForce = 300;


    public AudioSource sourceTick;
    public int nbTick = 5;
    public float tickPitchStart = 0.5f, tickPitchEnd = 2f;

    public AudioSource sourceJump;

    public AudioSource sourceWind;
    public float velocityAtWindPitch1 = 20;

    protected override void Awake()
    {
        inst = this;
        base.Awake();
    }

    protected override void UpdateController()
    {
        if (Input.GetKey(KeyCode.Z)) controller.x++;
        if (Input.GetKey(KeyCode.S)) controller.x--;
        if (Input.GetKey(KeyCode.D)) controller.y++;
        if (Input.GetKey(KeyCode.Q)) controller.y--;
    }

    protected override void Update()
    {
        base.Update();

        sourceWind.volume = rb.velocity.magnitude / velocityAtWindPitch1;
        sourceWind.pitch  = rb.velocity.magnitude / velocityAtWindPitch1;

        if (Input.GetKey(KeyCode.Space) && LoadJumpCoroutine == null && onGround)
            LoadJumpCoroutine = StartCoroutine(LoadJump());
    }



    Coroutine LoadJumpCoroutine = null;
    IEnumerator LoadJump()
    {
        float t = 0;
        float progress = 0;

        float[] ticksProgress = new float[nbTick];
        for (int i = 0; i < nbTick; i++)
            ticksProgress[i] = (float)i / (nbTick-1);
        int iTick = 0;


        while (Input.GetKey(KeyCode.Space))
        {
            t += Time.deltaTime;
            progress = loadJumpCurve.Evaluate(t/loadJumpTime);

            if (iTick < nbTick && progress > ticksProgress[iTick])
            {
                sourceTick.Play();
                sourceTick.pitch = Mathf.Lerp(tickPitchStart, tickPitchEnd, progress);
                iTick++;
            }

            yield return new WaitForEndOfFrame();
        }

        rb.AddForce(Vector3.up * jumpForce * progress);
        sourceJump.Play();

        LoadJumpCoroutine = null;
    }
}
