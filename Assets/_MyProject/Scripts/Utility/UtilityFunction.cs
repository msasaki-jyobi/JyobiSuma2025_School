using UnityEngine;

public class UtilityFunction : MonoBehaviour
{
    /// <summary>
    /// エフェクトを生成する
    /// </summary>
    /// <param name="root">エフェクトを生成する元のオブジェクト</param>
    /// <param name="effectPrefab">エフェクトプレハブ</param>
    /// <param name="loop">パーティクルシステムのループ</param>
    /// <param name="parentObject">設定したい親オブジェクト</param>
    public static void PlayEffect(GameObject root, GameObject effectPrefab, bool loop = false, GameObject parentObject = null, float destroyTime = 5f)
    {
        if (effectPrefab == null)
        {
            //Debug.LogWarning($"{root.gameObject.transform.name}:effectはありません");
            return;
        }
        // エフェクトを生成する
        GameObject effect = Instantiate(effectPrefab, root.transform.position, Quaternion.identity);
        // ループ設定を反映
        foreach (var particle in effect.gameObject.GetComponentsInChildren<ParticleSystem>())
            particle.loop = loop;
        // 親オブジェクトを設定
        if (parentObject != null)
        {
            //effect.transform.parent = parentObject.transform;
            effect.transform.SetParent(parentObject.transform, false);
            effect.transform.localPosition = Vector3.zero;
            effect.transform.rotation = parentObject.transform.rotation;
        }
        Destroy(effect, destroyTime);
    }

    /// <summary>
    /// LineCastを検知するメソッド
    /// </summary>
    public static bool CheckLineData(LineData lineData, Transform self)
    {
        if (lineData == null) return false;

        for (int i = 0; i < lineData.StartPosition.Count; i++)
        {
            // 向いている方向で座標を計算
            Vector3 lookStartPos =
                    self.transform.right * lineData.StartPosition[i].x +
                    self.transform.up * lineData.StartPosition[i].y +
                    self.transform.forward * lineData.StartPosition[i].z;
            Vector3 lookEndPos =
                    self.transform.right * lineData.EndPosition[i].x +
                    self.transform.up * lineData.EndPosition[i].y +
                    self.transform.forward * lineData.EndPosition[i].z;
            // 開始位置の座標
            Vector3 startPos = self.transform.position + lookStartPos;
            // 終了位置の座標
            Vector3 endPos = self.transform.position + lookEndPos;
            // 描画
            Debug.DrawLine(startPos, endPos, lineData.LineColor);
            // 判定チェック
            if (Physics.Linecast(startPos, endPos, lineData.LineLayer))
                return true;
        }
        return false;
    }
}
