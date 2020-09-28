# HelixSoundPhysics
Simple approximated dynamic sound physics (occlusion and reverb) for Unity.

# Usage
Add a HSPAudioSource component to an audio source and set its type depending on its usage.

Static for non moving audio sources, saves performance.

Dynamic for moving audio sources.

# Extra info

This asset is in an early stage and it may not be very performant or might have some issues. 

It will be optimized through development stages.

Using many dynamic HSPAudioSources may lower frame rates.

# Demo Scene setup

This package does not come with any audio tracks (i will make a simpler footstep demo later). 

In order to use the demo scene you must download and set the audio track in the audio source.
