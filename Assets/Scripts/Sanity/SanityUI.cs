using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SanityUI : MonoBehaviour
{
    public GameObject sanityDetails,
        addSanity;

    public Button sanityText,
        addSanityButton;

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject == sanityText.gameObject ||
            !EventSystem.current.currentSelectedGameObject == addSanityButton.gameObject)
        {
            sanityDetails.SetActive(false);
            addSanity.SetActive(false);
        }
    }

    /// <summary>
    /// ShowSanityDetails: Show sanity details UI when Sanity text is clicked
    /// </summary>
    public void ShowSanityDetails()
    {
        if(addSanity.activeInHierarchy)
            addSanity.SetActive(false); // Deactivate add Sanity
        
        // Activate sanityDetails when it is not active
        // Deactivate sanityDetails when it is active
        sanityDetails.SetActive(!sanityDetails.activeInHierarchy);
    }

    /// <summary>
    /// ShowAddSanity: Show add sanity UI when Add Sanity Button is clicked
    /// </summary>
    public void ShowAddSanity()
    {
        if(sanityDetails.activeInHierarchy)
            sanityDetails.SetActive(false); // Deactivate sanity Details
        
        // Activate addSanity when it is not active
        // Deactivate addSanity when it is active
        addSanity.SetActive(!addSanity.activeInHierarchy);
    }
}
