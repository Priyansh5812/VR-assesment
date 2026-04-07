using UnityEngine;
using UnityEngine.VFX;

namespace UnityStandardAssets.Effects
{
    public class ParticleSystemMultiplier : MonoBehaviour
    {
        // a simple script to scale the size, speed and lifetime of a particle system



        public float _pMultiplier = 1;
        public float _lMultiplier = 1;
        public float _vMultiplier = 2;
        public float temp = 1;
        public ParticleSystem[] systems;
        public VisualEffect[] effects;
        public float currentLerpTime = 0f;
        public float ExtinguishDuration = 5f;
        public Light[] lights;
        public bool isExtinguished;
        public float[] sizeMuls, speedMuls, lifeMuls, LightIns , smokeMuls , sparkMuls;
        public float[] tsizeMuls, tspeedMuls, tlifeMuls, tLightIns, tsmokeMuls, tsparkMuls;

        private void Start()
        {   
            isExtinguished = false;
            systems = GetComponentsInChildren<ParticleSystem>();
            lights = GetComponentsInChildren<Light>();
            effects = GetComponentsInChildren<VisualEffect>();
            sizeMuls = new float[systems.Length];
            speedMuls = new float[systems.Length];
            lifeMuls = new float[systems.Length];
            LightIns = new float[lights.Length];
            smokeMuls = new float[effects.Length];
            sparkMuls = new float[effects.Length];
            
            tsizeMuls = new float[systems.Length];
            tspeedMuls = new float[systems.Length];
            tlifeMuls = new float[systems.Length];
            tLightIns = new float[lights.Length];
            tsmokeMuls = new float[effects.Length];
            tsparkMuls = new float[effects.Length];


            int i = 0;
            foreach (ParticleSystem system in systems)
            {
                ParticleSystem.MainModule mainModule = system.main;
                sizeMuls[i] = mainModule.startSizeMultiplier;
                speedMuls[i] = mainModule.startSpeedMultiplier;
                lifeMuls[i] = mainModule.startLifetimeMultiplier;
                i++;
                system.Clear();
                system.Play();
            }
            i = 0;
            foreach(var l in lights)
            {
                LightIns[i++] = l.intensity;
            }
            i = 0;
            foreach(var e in effects)
            {
                smokeMuls[i] = e.GetFloat("SmokeMultiplier");
                sparkMuls[i++] = e.GetFloat("SparkMultiplier");
            }

            tsizeMuls = sizeMuls.Clone() as float[];
            tspeedMuls = speedMuls.Clone() as float[];
            tlifeMuls = lifeMuls.Clone() as float[];
            tLightIns = LightIns.Clone() as float[];
            tsmokeMuls = smokeMuls.Clone() as float[];
            tsparkMuls = sparkMuls.Clone() as float[];
            
        }


        //public void ExtinguishFire() //Will pass extinguish duration as parameter
        //{

        //    SetMultipliers();

        //    for (int i = 0; i < systems.Length; i++)
        //    {
        //        ParticleSystem.MainModule mainModule = systems[i].main;
        //        mainModule.startSizeMultiplier = tsizeMuls[i];
        //        mainModule.startSpeedMultiplier = tspeedMuls[i];
        //        mainModule.startLifetimeMultiplier = tlifeMuls[i];
        //    }


        //    for (int i = 0; i < lights.Length; i++)
        //    {
        //        lights[i].intensity = tLightIns[i];
        //    }

        //    for(int i =0;i < effects.Length; i++)
        //    {
        //        effects[i].SetFloat("SmokeMultiplier", tsmokeMuls[i]);
        //        effects[i].SetFloat("SparkMultiplier", tsparkMuls[i]);
        //    }

        //    if (currentLerpTime == ExtinguishDuration)
        //    {
        //        isExtinguished = true;
        //        Debug.Log(GameManager.Instance.state);
        //        if (GameManager.Instance.state == GameState.InLobby)
        //        {   
        //            GameManager.Instance.uiManager.IncrementCounter();
        //        }
        //    }
        //}

        //private void SetMultipliers()
        //{
        //    currentLerpTime += Time.deltaTime;
        //    if (currentLerpTime > ExtinguishDuration)
        //    {
        //        currentLerpTime = ExtinguishDuration;
        //    }

        //    float progress = currentLerpTime / ExtinguishDuration;

        //    for(int i = 0; i < systems.Length; i++)
        //    {
        //        tsizeMuls[i] = Mathf.Lerp(sizeMuls[i], 0f, progress);  
        //        tspeedMuls[i] = Mathf.Lerp(speedMuls[i], 0f, progress);  
        //        tlifeMuls[i] = Mathf.Lerp(lifeMuls[i], 0f, progress);  
        //    }

        //    for (int i = 0; i < lights.Length; i++)
        //    {
        //        tLightIns[i] = Mathf.Lerp(LightIns[i], 0f, progress);
        //    }

        //    for (int i = 0; i < effects.Length; i++)
        //    {
        //        tsmokeMuls[i] = Mathf.Lerp(smokeMuls[i], 0f, progress);
        //        tsparkMuls[i] = Mathf.Lerp(sparkMuls[i], 0f, progress);
        //    }


        //}
    }
}
