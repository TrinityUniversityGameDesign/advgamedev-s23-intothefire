using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float damage = 0f;

    [SerializeField]
    float dot = 0f;

    [SerializeField]
    float knockback = 5f;

    [SerializeField]
    bool lifesteal = false;
    float lifestealVal = 0;
    float health = 5f;
    float maxHealth = 5;
    float repeatTimer = 0;
    float currBurn;
    float burnTime = 0;
    float lifegain = 0;
    bool canBeKnockedback = false;
    float knockbackResist = 0;
    float armor = 0;
    GameObject parent;
    GameObject player;
    Rigidbody rb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (repeatTimer >= 50)
        {
            repeatTimer = 0;
            if (currBurn > 0)
            {
                health -= Mathf.Ceil(currBurn / 10f);
                //burnTime--;
                currBurn -= Mathf.Ceil(currBurn / 10f);
                if (currBurn <= 0)
                {
                    currBurn = 0;
                    burnTime = 0;
                }
            }
            health += lifegain;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
            
        }
        else
        {
            repeatTimer++;
        }
    }
    public void SetDamage(float f)
    {
        damage = f;
    }
    public float GetDamage()
    {
        return damage;
    }

    public void SetKnockback(float f)
    {
        knockback = f;
    }
    public float GetKnockback()
    {
        return knockback;
    }

    public void SetDamageOverTime(float f)
    {
        dot = f;
    }

    public float GetDamageOverTime()
    {
        return dot;
    }

    public void DoLifesteal(GameObject play)
    {
        player = play;
        lifesteal = true;
    }

    public bool GetLifesteal()
    {
        return lifesteal;
    }
    public void SetParent(GameObject g)
    {
        parent = g;
    }
    public GameObject GetParent()
    {
        return parent;
    }

    public void SetHealth(float h)
    {
        health = h;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(float h)
    {
        maxHealth = h;
    }
    public void TakeDamage(GameObject other)
    {
        DamageScript temp = other.GetComponent<DamageScript>();
        float hurts = Mathf.Max(0f, (temp.GetDamage() - armor));
        health -= hurts;
        
        currBurn = temp.GetDamageOverTime();
        burnTime = 10;
        if (temp.GetLifesteal())
        {
            if (other.gameObject.tag == "Player")
            {
                other.GetComponentInParent<JacksonCharacterMovement>().StealLife(hurts);
            }
            else
            {
                other.GetComponent<DamageScript>().StealLife(hurts);
            }
        }
        
        
        transform.LookAt(new Vector3(other.transform.position.x, transform.position.y - 1f, other.transform.position.z));
        
      
        //grounded = false;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        //stunTimer = 10f;
        if (canBeKnockedback)
        {
            float kb = temp.GetKnockback() - knockbackResist;
            if (kb < 1) { kb = 1f; }
            
            transform.LookAt(new Vector3(other.transform.position.x, transform.position.y - 1f, other.transform.position.z));

            kb *= -20;
            GetComponent<Rigidbody>().velocity = kb * transform.forward;
        }
        
        
    }
    public void TakeKnockback(bool b)
    {
        canBeKnockedback = b;
    }
    public void SetLifegain(float l)
    {
        lifegain = l;
    }
    public float GetLifegain()
    {
        return lifegain;
    }
    public void SetLifestealVal(float v)
    {
        lifestealVal = v;
    }
    public float GetLifestealVal()
    {
        return lifestealVal;
    }
    public void StealLife(float h)
    {
        health += Mathf.Ceil(h*lifestealVal);
        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (/*other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" ||*/ other.gameObject.tag == "Damage")
        {
            if (parent == null || (parent != null && parent.tag != "Player"))
            {
                TakeDamage(other.gameObject);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(/*other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || */other.gameObject.tag == "Damage")
        {
            if(parent == null || (parent != null && parent.tag != "Player"))
            {
                TakeDamage(other.gameObject);
            }
            
        }
    }
}
