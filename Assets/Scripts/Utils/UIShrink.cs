using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIShrink : MonoBehaviour {

	public enum AspectRatioPreservationModes { None, Partial, Full }

	public bool useThresholdResolution = false;
	public Vector2 thresholdResolution;

	public AspectRatioPreservationModes aspectRatioPreservation = AspectRatioPreservationModes.Partial;

	private Vector2 nativeResolution;
	private RectTransform rectTransform;
    private Vector2 prevParentLocalSize = Vector2.zero;

	private void Start() {
		rectTransform = GetComponent<RectTransform>();

		nativeResolution = rectTransform.GetLocalSize();

        //ScreenResizeEvent.onResizeStatic.AddListener(Rescale);

		//Rescale();
	}

    void Update()
    {
        Vector2 parentLocalSize = (rectTransform.parent as RectTransform).rect.size;
        if (prevParentLocalSize != parentLocalSize)
        {
            Rescale();
            prevParentLocalSize = parentLocalSize;
        }
    }

    public void Rescale() {
        if (rectTransform == null) return;//Haven't started yet

        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;

		rectTransform.offsetMin = Vector2.zero;
		rectTransform.offsetMax = Vector2.zero;

		rectTransform.localScale = Vector3.one;

		Vector2 size = rectTransform.GetLocalSize();

		float nativeRatio = nativeResolution.x / nativeResolution.y;

		Vector2 offsetDisplacement = Vector2.zero;

		if (aspectRatioPreservation == AspectRatioPreservationModes.Full) {

            Vector2 ratioSize;
            if (nativeRatio < size.x / size.y) {
                ratioSize = new Vector2(size.y * nativeRatio, size.y);
            } else {
                ratioSize = new Vector2(size.x, size.x / nativeRatio);
            }

			offsetDisplacement = new Vector2((size.x - ratioSize.x) / 2, (size.y - ratioSize.y) / 2);

			size = ratioSize;
		}

		if (useThresholdResolution) {
			if (size.x >= thresholdResolution.x && size.y >= thresholdResolution.y) {
				return;
			}
			SetLocalScale(size, thresholdResolution);
		}
		else {
			SetLocalScale(size, nativeResolution);
		}

        rectTransform.SetScaledOffsets(offsetDisplacement, -offsetDisplacement);
    }

	private void SetLocalScale(Vector2 size, Vector2 resolution) {
		if (aspectRatioPreservation != AspectRatioPreservationModes.None) {
			float minScale = (size.x / resolution.x <= size.y / resolution.y) ? (size.x / resolution.x) : (size.y / resolution.y);

			rectTransform.localScale = new Vector3(minScale, minScale, 1);
		}
		else {
			rectTransform.localScale = new Vector3(size.x / resolution.x, size.y / resolution.y, 1);
		}
	}
}