using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Helix.Audio
{   [RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioReverbFilter))]
    public class HSPAudioSource : HSPBase
    {
        AudioSource uSource;
        AudioListener listener;
        public enum SampleCount
        {
            Low = 10,
            Medium = 8,
            High = 6,
            Extreme = 4
        }
        SampleCount samples = SampleCount.Low;
        public enum SoundType
        {
            Static,
            Dynamic
        }
        public SoundType soundType;
        float distance;
        bool baked;
        
        AudioLowPassFilter lp;
        AudioReverbFilter echo;
        void Start()
        {
            uSource = GetComponent<AudioSource>();
            listener = FindObjectOfType<AudioListener>();

            lp = GetComponent<AudioLowPassFilter>();
            echo = GetComponent<AudioReverbFilter>();
         
        }

        // Update is called once per frame
        void FixedUpdate()
        {
          base.FixedUpdate();
        }
        public override void OnHSPUpdate()
        {
            distance = 0;
            RaycastHit hitFrom;
            RaycastHit hitTo;
            if (Physics.Linecast(transform.position, listener.transform.position, out hitFrom))
            {
                Debug.DrawLine(transform.position, hitFrom.point, Color.red, 1f);
                if (Physics.Linecast(listener.transform.position, transform.position, out hitTo))
                {
                    Debug.DrawLine(listener.transform.position, hitTo.point, Color.green, 1f);
                  
                    lp.cutoffFrequency = Mathf.Lerp(lp.cutoffFrequency,1000 - 500 * Vector3.Distance(hitFrom.point, hitTo.point),0.8f);
                }
            }
            else
            {
                lp.cutoffFrequency = Mathf.Lerp(lp.cutoffFrequency, 22000, 0.8f);
            }
            if (soundType == SoundType.Dynamic||soundType==SoundType.Static&&!baked)
            {
                baked = true;
                Ray ray = new Ray();
                ray.origin = transform.position;
                float inverseResolution = (int)samples;
                Vector3 direction = Vector3.right;
                int steps = Mathf.FloorToInt(360f / inverseResolution);
                Quaternion xRotation = Quaternion.Euler(Vector3.right * inverseResolution);
                Quaternion yRotation = Quaternion.Euler(Vector3.up * inverseResolution);
                Quaternion zRotation = Quaternion.Euler(Vector3.forward * inverseResolution);
                RaycastHit hit;
                for (int x = 0; x < steps / 2; x++)
                {
                    direction = zRotation * direction;
                    for (int y = 0; y < steps; y++)
                    {
                        direction = xRotation * direction;
                        if (Physics.Raycast(transform.position, direction, out hit))
                        {
                            Debug.DrawLine(ray.origin, ray.origin + direction * hit.distance, Color.white, HSPConfig.audioStep + 0.02f);
                            distance += hit.distance / steps;
                        }
                        else
                        {
                            distance = 0;
                            break;
                        }
                    }
                }
                echo.room = -2000 + ((distance/310) * 1500);
                echo.reflectionsLevel = -2000 + ((distance/310) * 1500);
                echo.reflectionsDelay = distance / 3400;
                echo.reverbDelay = distance / 3400;
            }
        }
        public void Play()
        {
            uSource.PlayDelayed(Vector3.Distance(transform.position, listener.transform.position) / 340);
        }
         
        }
    }
