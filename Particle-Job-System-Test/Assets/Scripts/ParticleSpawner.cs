using UnityEditor;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public ParticleSystem prefab;
    public int systemCount = 128;
    public int particleEmissionRatePerSystem = 400;
    public float particleSystemShapeRadius = 1.0f;
    public float totalRadius = 5.0f;
    public float effectRange = 3.0f;
    public float effectStrength = 1.0f;
    public float oscillationSpeed = 10.0f;
    public bool hasTrails = true;
    public bool useJobSystem = false;

    void Start()
    {
        var material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        material.SetTexture("_MainTex", AssetDatabase.GetBuiltinExtraResource<Texture2D>("Default-Particle.psd"));

        for (int i = 0; i < systemCount; i++)
        {
            var go = GameObject.Instantiate(prefab);
            var ps = go.GetComponent<ParticleSystem>();

            go.GetComponent<ParticleSystemRenderer>().sharedMaterial = material;

            float x = (float)i / systemCount;
            float theta = x * Mathf.PI * 2;

            var transform = go.GetComponent<Transform>();
            transform.position = new Vector3(Mathf.Sin(theta), Mathf.Cos(theta), 0.0f) * totalRadius;

            var main = ps.main;
            main.startColor = Color.HSVToRGB(x, Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f));

            var emission = ps.emission;
            emission.rateOverTime = particleEmissionRatePerSystem;

            var shape = ps.shape;
            shape.radius = particleSystemShapeRadius;

            var trails = ps.trails;
            trails.enabled = hasTrails;

            var updateJob = go.GetComponent<ParticleJob>();
            updateJob.effectRange = effectRange;
            updateJob.effectStrength = effectStrength;
            updateJob.oscillationSpeed = oscillationSpeed;
            updateJob.useJobSystem = useJobSystem;
        }
    }

    // UI
    void OnGUI()
    {
        float x = 25.0f;
        float y = 60.0f;
        float spacing = 40.0f;

        EditorGUI.BeginChangeCheck();

        GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
        backgroundStyle.normal.background = Texture2D.whiteTexture;
        var oldColor = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        GUI.Box(new Rect(x - 10, y - 35, 260, 230), "Options", backgroundStyle);
        GUI.backgroundColor = oldColor;

        GUI.Label(new Rect(x, y, 140, 30), "Effect Range");
        effectRange = GUI.HorizontalSlider(new Rect(x + 140, y + 5, 100, 30), effectRange, 0.0f, 10.0f);
        y += spacing;

        GUI.Label(new Rect(x, y, 140, 30), "Effect Strength");
        effectStrength = GUI.HorizontalSlider(new Rect(x + 140, y + 5, 100, 30), effectStrength, 0.0f, 10.0f);
        y += spacing;

        GUI.Label(new Rect(x, y, 140, 30), "Oscillation Speed");
        oscillationSpeed = GUI.HorizontalSlider(new Rect(x + 140, y + 5, 100, 30), oscillationSpeed, 0.0f, 20.0f);
        y += spacing;

        hasTrails = GUI.Toggle(new Rect(x, y + 5, 140, 30), hasTrails, "Trails");
        y += spacing;

        useJobSystem = GUI.Toggle(new Rect(x, y + 5, 140, 30), useJobSystem, "Use C# Job System");
        y += spacing;

        if (EditorGUI.EndChangeCheck())
        {
            ParticleJob[] updateJobs = GameObject.FindObjectsOfType<ParticleJob>();
            for (int i = 0; i < updateJobs.Length; i++)
            {
                var updateJob = updateJobs[i];
                updateJob.effectRange = effectRange;
                updateJob.effectStrength = effectStrength;
                updateJob.oscillationSpeed = oscillationSpeed;
                updateJob.useJobSystem = useJobSystem;

                var ps = updateJob.GetComponent<ParticleSystem>();

                var trails = ps.trails;
                trails.enabled = hasTrails;
            }
        }
    }
}