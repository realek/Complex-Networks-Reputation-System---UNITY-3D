using UnityEngine;

public class NpcDeathVisual : MonoBehaviour {

    private ParticleSystem ps;
    private bool started;
	// Use this for initialization
	void Awake () {

        ps = GetComponent<ParticleSystem>();
	}

    public void RunOnce()
    {
        if (!started)
        {
            ps.Play();
            started = true;
        }
    }
    private void Update()
    {
        if (started && !ps.isPlaying)
            Destroy(gameObject);
    }
}
