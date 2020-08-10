using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

public class HeroMovement : MonoBehaviour
{
    public SoundManager Sounds;
    public Animator anim;
    public LineRenderer lineRender;
    public GameObject DestroyCollider;
    public ParticleSystem JetpackEmitter;
    public GameObject FlashLight;

    public Inventory HeroInventory;
    public ItemDrop drop;

    public InGameOverlayManager InGameOverlay;

    public Material dissolve;
    public List<Tool> tools_start = new List<Tool>() { };
    public BoxCollider2D PlayerCollider;
    public LayerMask BlocksLayerMask;

    Vector3 null_point = new Vector3(0, 0, 0);
    HeroParameters hero;
    Rigidbody2D rb;
    Light2D FlashLightLight;

    void Start()
    {   
        //Application.targetFrameRate = 120;
        hero = GetComponent<HeroParameters>();
        transform.position = new Vector2(1, 15);
        HeroInventory.AddItemHud(HeroInventory.Hud_Cells[0], tools_start[0].gameObject, 1);
        rb = GetComponent<Rigidbody2D>();

        FlashLightLight = FlashLight.GetComponent<Light2D>();

        dissolve.SetFloat("_Fade", 0);

    }

    public void Spawn(Vector2 spawn)
    {
        transform.position = spawn;
        //hero.SpawnPoint = spawn;
    }

    Collider2D IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.Raycast(PlayerCollider.bounds.center, Vector2.down, PlayerCollider.bounds.extents.y + .1f, BlocksLayerMask);
        return raycastHit.collider;
    }

    void Update()
    {
        Application.targetFrameRate = 1000;
        if (hero.hp <= 0)
        {
            Respawn();
        }

        if (hero.Can_move)
        {
            // Проверка на прыжок.
            if (Input.GetButton("Jump") && IsGrounded())
            {
                jump();
                anim.SetBool("Jump", true);
                hero.is_grounded = false;
            }
        }

        if (Input.GetButtonDown("Flashlight"))
        {
            FlashLight.SetActive(!FlashLight.activeSelf);
        }

        if (Input.GetButton("Speedup"))
        {
            hero.player_speed = 10f;
        }

        else
        {
            hero.player_speed = 5f;
        }

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        if (Input.GetButton("Hit") || Input.GetMouseButton(0))
        {
            anim.SetBool("Hit", true);
        }   else
        {
            anim.SetBool("Hit", false);
            DestroyCollider.transform.position = null_point;
        }

        if (Input.GetAxis("Vertical") > 0.1f && !hero.SteamEnded)
        {
            if (hero.steam - hero.SteamSubAmount * Time.deltaTime <= 0f)
            {
                hero.steam = 0;
                hero.SteamEnded = true;
            }
            else
            {
                hero.steam -= hero.SteamSubAmount * Time.deltaTime;
                Vector2 vel = new Vector2(rb.velocity.x, Input.GetAxis("Vertical") * 8);
                rb.velocity = vel;
                JetpackEmitter.enableEmission = true;
                anim.SetBool("Jump", true);
            }

        }
        else
        {

            JetpackEmitter.enableEmission = false;
            hero.steam += hero.SteamAddAmount * Time.deltaTime;

            if (hero.steam + hero.SteamAddAmount * Time.deltaTime >= 100f)
            {
                hero.steam = 100;
            }

            if (hero.steam >= 30f)
            {
                hero.SteamEnded = false;
            }
        }

        dissolve.SetFloat("_Fade", Mathf.Lerp(dissolve.GetFloat("_Fade"), hero.player_opacity, 1 * Time.deltaTime));
        InGameOverlay.HPsr.value = hero.hp;
        InGameOverlay.STEAMsr.value = hero.steam;
        InGameOverlay.STEAMnum.text = ((int)(hero.steam / 10)).ToString();
        InGameOverlay.HPnum.text = ((int)(hero.hp / 10)).ToString();
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Falling speed", -rb.velocity.y);
    }

    void FixedUpdate()
    {
        // Проверка на возможность движения.
        //if (Input.GetButton("Hit"))
        if (false)
        {
            Vector2 pos1 = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Vector2 pos2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * 1 + pos1;
            RenderLine(pos1, pos2);
            anim.SetBool("Hit", true);

        }
        else
        {
            DestroyLine();
            if (hero.Can_move)
            {
                Vector2 vel = new Vector2(Input.GetAxis("Horizontal") * hero.player_speed, rb.velocity.y);
                if ((Mathf.Abs(vel.x) > 0.5 || Mathf.Abs(vel.y) > 0.5) && IsGrounded() != null && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.W))
                {
                    Sounds.SetList(IsGrounded().gameObject.name);
                    Sounds.PlayStep();
                }
                rb.velocity = vel;
            }
        }
    }

    void jump()
    {
        // Обычный прыжок.   
        rb.velocity = new Vector2(0, 15);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Если человек прикасается к какому-либо объекту.
        if (IsGrounded() != null)
        {
            hero.is_grounded = true;
            anim.SetBool("Jump", false);
        }
        if (coll.transform.tag == "mob")
        {
            Hit(coll.gameObject.GetComponent<Mob>().my_damage);
        }
    }

    private void RenderLine(Vector2 startPoint, Vector2 endPoint)
    {
        lineRender.positionCount = 2;
        Vector3[] points = new Vector3[2];
        points[0] = new Vector3(startPoint.x, startPoint.y, 0);
        points[1] = new Vector3(endPoint.x, endPoint.y, 0);
        lineRender.SetPositions(points);
        DestroyCollider.transform.position = endPoint;
    }

    public void Hit(float hit)
    {
        hero.hp -= hit;
    }

    private void DestroyLine()
    {
        lineRender.positionCount = 0;
    }

    void Respawn()
    {
        Vector2 old_position = transform.position + new Vector3(10, 10);
        Spawn(hero.SpawnPoint);
        hero.hp = 1000f;
        for (int cell_i = 0; cell_i < HeroInventory.Cells.Count; cell_i++)
        {
            if (HeroInventory.Cells[cell_i].is_item)
            {
                while (HeroInventory.Cells[cell_i].count_item > 0)
                {
                    drop.GenerateItemDrop(HeroInventory.Cells[cell_i].object_save, old_position);
                    HeroInventory.Cells[cell_i].count_item -= 1;
                }
            }
        }

    }
}
