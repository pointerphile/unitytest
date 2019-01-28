using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PP {
    public class PPScreenFader : MonoBehaviour
    {
        private static PPScreenFader m_Instance = null;
        private Material m_Material;
        private string m_LevelName = "";
        private int m_LevelIndex = 0;
        private bool m_Fading = false;
        private static PPScreenFader Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = (new GameObject("PPScreenFader")).AddComponent<PPScreenFader>();
                }
                return m_Instance;
            }
        }
        public static bool Fading
        {
            get { return Instance.m_Fading; }
        }

        private void Awake()
        {
            // 씬이 변경되어도 삭제하지 않는다.
            DontDestroyOnLoad(this);
            m_Instance = this;
            m_Material = Resources.Load<Material>("Materials/MtlAutoFade");
        }

        private void DrawQuad(Color aColor, float aAlpha)
        {
            if (!m_Material)
            {
                return;
            }
            GL.PushMatrix();
            m_Material.SetPass(0);
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Color(new Vector4(aColor.r, aColor.g, aColor.b, aAlpha));
            GL.Vertex3(0, 0, -1);
            GL.Vertex3(0, 1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, 0, -1);
            GL.End();
            GL.PopMatrix();
        }

        private IEnumerator Fade(float aFadeOutTime, float aFadeInTime, Color aColor)
        {
            float t = 0.0f;
            while (t < 1.0f)
            {
                // 모든 카메라와 GUI가 렌더링을 완료하는 프레임의 마지막을 기다린다.
                yield return new WaitForEndOfFrame();
                t = Mathf.Clamp01(t + Time.deltaTime / aFadeOutTime);
                DrawQuad(aColor, t);
                Debug.Log("fade : " + t);
            }
            if (m_LevelName != "")
                SceneManager.LoadScene(m_LevelName);
            else
                SceneManager.LoadScene(m_LevelIndex);
            while (t > 0.0f)
            {
                yield return new WaitForEndOfFrame();
                t = Mathf.Clamp01(t - Time.deltaTime / aFadeInTime);
                DrawQuad(aColor, t);
                Debug.Log("fade : " + t);
            }
            m_Fading = false;
        }
        private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
        {
            m_Fading = true;
            StartCoroutine(Fade(aFadeOutTime, aFadeInTime, aColor));
        }

        public static void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
        {
            if (Fading) return;
            Instance.m_LevelName = aLevelName;
            Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
        }
        public static void LoadLevel(int aLevelIndex, float aFadeOutTime, float aFadeInTime, Color aColor)
        {
            if (Fading) return;
            Instance.m_LevelName = "";
            Instance.m_LevelIndex = aLevelIndex;
            Instance.StartFade(aFadeOutTime, aFadeInTime, aColor);
        }
    }

}