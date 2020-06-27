using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessManager : MonoBehaviour
{
    public GameObject cameraGameObject;

    PostProcessingProfile postProcessingProfile;

    // Start is called before the first frame update
    void Awake()
    {
        postProcessingProfile = cameraGameObject.GetComponent<PostProcessingBehaviour>().profile;
    }

    public void SetChroma(float targetChroma, float speed)
    {
        ChromaticAberrationModel.Settings chroma = postProcessingProfile.chromaticAberration.settings;

        float originalChroma = chroma.intensity;
        chroma.intensity = Mathf.Lerp(originalChroma, targetChroma, speed * Time.deltaTime);

        postProcessingProfile.chromaticAberration.settings = chroma;
    }

    public void SetTemperature(float temperature)
    {
        ColorGradingModel.Settings cog = postProcessingProfile.colorGrading.settings;
        cog.basic.temperature = temperature;
        postProcessingProfile.colorGrading.settings = cog;
    }
}
