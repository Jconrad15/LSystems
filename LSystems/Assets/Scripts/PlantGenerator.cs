using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSystems
{
    public class PlantGenerator : MonoBehaviour
    {
        [SerializeField]
        private Material lineMaterial;

        [SerializeField]
        private Texture2D leafTexture;

        private Vector3 currentDrawPosition = Vector3.zero;
        private Stack<Vector3> returnDrawPositionStack = new Stack<Vector3>();

        private Vector3 direction;
        private Stack<Vector3> returnDirectionStack = new Stack<Vector3>();

        private const float colorGradientAmount = 0.0001f;

        [Range(1, 6)]
        [SerializeField]
        private int iterations = 4;

        public void SetIterations(System.Single newIterations)
        {
            iterations = (int)newIterations;
        }

        [Range(1, 90)]
        [SerializeField]
        private float rotationAngle = 25f;

        public void SetRotationAngle(System.Single newRotationAngle)
        {
            rotationAngle = newRotationAngle;
        }

        [Range(0.1f, 0.5f)]
        [SerializeField]
        private float lineLength = 0.2f;

        public void SetLineLength(System.Single newLineLength)
        {
            lineLength = newLineLength / 10f;
        }

        [Range(0.01f, 0.2f)]
        [SerializeField]
        private float lineWidth = 0.1f;
        public void SetLineWidth(System.Single newLineWidth)
        {
            lineWidth = newLineWidth / 10f;
        }

        [SerializeField]
        private Color32 lineColor = new Color32(173, 73, 31, 255);

        [SerializeField]
        private string axiom = "X";

        public void SetAxiom(string newAxiom)
        {
            axiom = newAxiom;
        }

        [SerializeField]
        private string XRule = "FL+[[X]-X]-F[-FLX]+LFLX";

        public void SetXRule(string newXRule)
        {
            XRule = newXRule;
        }

        [SerializeField]
        private string YRule = "[+Y]F[-Y]FY";

        [SerializeField]
        private string ZRule = "[+Z]F[-Z]+Z";

        [SerializeField]
        private string FRule = "FF";

        public void SetFRule(string newFRule)
        {
            FRule = newFRule;
        }
        //private string[] axioms = new string[6] { "X", "-X", "+X", "W", "Y", "Z"};

        private GameObject topParent;
        private GameObject plantParent;

        public GameObject CreatePlant(bool isCoroutine)
        {
            GameObject plant = new GameObject("Plant");
            plant.transform.SetParent(gameObject.transform);
            topParent = plantParent = plant;

            ResetStartingVectors();

            // Create plant string by applying rules
            string plantString = ApplyRules(iterations, axiom);
            //Debug.Log(plantString);

            // Generate the plant based on the plant string
            if (isCoroutine == true)
            {
                StartCoroutine(DrawCoroutine(plantString));
            }
            else
            {
                Draw(plantString);
            }

            return topParent;
        }

        private void ResetStartingVectors()
        {
            currentDrawPosition = Vector3.zero;
            direction = Vector3.up;
            returnDirectionStack.Clear();
            returnDrawPositionStack.Clear();
        }

        private string ApplyRules(int iterations, string axiom)
        {
            string currentString = axiom;

            // For each itertion
            for (int i = 0; i < iterations; i++)
            {
                // Create a string for this iteration
                string iterationString = null;

                // For each character in the current string
                foreach (char c in currentString)
                {
                    // Check if there is a rule for c and apply it
                    string ruleOutput = CheckRule(c);

                    iterationString += ruleOutput;
                }

                // After each character is evaluated
                // Set the currentString to the iterationString
                currentString = iterationString;
            }

            return currentString;
        }

        private string CheckRule(char c)
        {
            if (c == 'X')
            {
                return XRule;
            }
            else if (c == 'Y')
            {
                return YRule;
            }
            else if (c == 'Z')
            {
                return ZRule;
            }
            else if (c == 'F')
            {
                return FRule;
            }
            else
            {
                // No rule for this character. 
                return c.ToString();
            }

        }

        private void Draw(string plantString)
        {
            foreach (char c in plantString)
            {
                ApplyDrawingRule(c);
            }
        }

        private IEnumerator DrawCoroutine(string plantString)
        {
            foreach (char c in plantString)
            {
                ApplyDrawingRule(c);
                yield return null;
            }
        }

        private void ApplyDrawingRule(char c)
        {
            if (c == 'F')
            {
                Vector3 startPos = currentDrawPosition;
                Vector3 endPos = currentDrawPosition + direction;

                CreateLine(startPos, endPos);

                // Return position and direction
                currentDrawPosition = endPos;

                ResizeDirection();
            }
            else if (c == 'L')
            {
                CreateLeaf(currentDrawPosition);
            }
            else if (c == '+')
            {
                // Change the direction by a set rotation (e.g., 45 degrees)
                direction = RotateDirection(rotationAngle);
                ResizeDirection();
            }
            else if (c == '-')
            {
                // Change the direction by a set rotation (e.g., 45 degrees)
                direction = RotateDirection(-rotationAngle);
                ResizeDirection();
            }
            else if (c == '[')
            {
                // Add the position to the stack
                returnDrawPositionStack.Push(currentDrawPosition);
                // Add the direction to the stack
                returnDirectionStack.Push(direction);

                ResizeDirection();
            }
            else if (c == ']')
            {
                // Return position and direction
                currentDrawPosition = returnDrawPositionStack.Pop();
                direction = returnDirectionStack.Pop();

                ResizeDirection();
            }
            else
            {
                // No rule for this character. 
            }
        }

        private Vector3 RotateDirection(float angle)
        {
            Vector3 tempAngle = Vector3.zero;

            tempAngle.x = direction.x * Mathf.Cos(Mathf.Deg2Rad * angle) -
                          direction.y * Mathf.Sin(Mathf.Deg2Rad * angle);

            tempAngle.y = direction.x * Mathf.Sin(Mathf.Deg2Rad * angle) +
                          direction.y * Mathf.Cos(Mathf.Deg2Rad * angle);

            return tempAngle;
        }

        private void ResizeDirection()
        {
            direction.Normalize();
            direction *= lineLength;
        }

        private void CreateLine(Vector3 startPos, Vector3 endPos)
        {
            GameObject newLine_go = new GameObject("Line");
            newLine_go.transform.SetParent(plantParent.transform);
            LineRenderer newLine_lr = newLine_go.AddComponent<LineRenderer>();

            // Set the color
            newLine_lr.material = lineMaterial;

            newLine_lr.startColor = lineColor;
            UpdateColor(lineColor);
            newLine_lr.endColor = lineColor;

            newLine_lr.startWidth = lineWidth;
            newLine_lr.endWidth = lineWidth;

            newLine_lr.SetPosition(0, startPos);
            newLine_lr.SetPosition(1, endPos);

            newLine_lr.sortingLayerName = "Branches";
        }

        private void CreateLeaf(Vector3 startPos)
        {
            GameObject newLeaf_go = new GameObject("Leaf");
            newLeaf_go.transform.SetParent(plantParent.transform);
            newLeaf_go.transform.position = startPos;
            newLeaf_go.transform.localScale = new Vector3(0.1f, 0.1f, 1);

            float angle = Mathf.Acos(
                Vector2.Dot(
                    new Vector2(startPos.x, startPos.y).normalized,
                    new Vector2(direction.x, direction.y).normalized));

            newLeaf_go.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);

            SpriteRenderer newLeaf_sr = newLeaf_go.AddComponent<SpriteRenderer>();

            newLeaf_sr.sprite = Sprite.Create(
                leafTexture,
                new Rect(0.0f, 0.0f, leafTexture.width, leafTexture.height),
                new Vector2(0.5f, 0));
            newLeaf_sr.sortingLayerName = "Leaves";
        }

        private void UpdateColor(Color32 oldColor)
        {
            // Get current color
            float h; float s; float v;
            Color.RGBToHSV(oldColor, out h, out s, out v);

            // Edit color
            v += colorGradientAmount;

            // Set new color
            Color32 newColor = Color.HSVToRGB(h, s, v);
            lineColor = newColor;

        }
    }



}
