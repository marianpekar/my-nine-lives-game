using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcesingManager : MonoBehaviour
{
    public GameObject cameraGameObject;

    PostProcessingProfile postProcessingProfile;

    // Start is called before the first frame update
    void Start()
    {
        postProcessingProfile = cameraGameObject.GetComponent<PostProcessingBehaviour>().profile;
    }

    public void SetDOF(float targetAperture, float targetFocusDistance, float speed)
    {
        DepthOfFieldModel.Settings dof = postProcessingProfile.depthOfField.settings;

        float originalfStop = dof.aperture;
        dof.aperture = Mathf.Lerp(originalfStop, targetAperture, speed * Time.deltaTime);

        float originalFocusDistance = dof.focusDistance;
        dof.focusDistance = Mathf.Lerp(originalFocusDistance, targetFocusDistance, speed * Time.deltaTime);

        postProcessingProfile.depthOfField.settings = dof;
    }
}
