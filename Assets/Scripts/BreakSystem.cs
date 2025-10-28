using UnityEngine;

public class BreakSystem : MonoBehaviour
{
    public Sprite sourceSprite;
    public int pieces = 6;

    private void Start()
    {
        sourceSprite = GetComponent<SpriteRenderer>().sprite;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Break()
    {
        if (sourceSprite == null)
            sourceSprite = GetComponent<SpriteRenderer>().sprite;

        Texture2D tex = sourceSprite.texture;
        Rect spriteRect = sourceSprite.rect; // 이 스프라이트의 위치/크기 (픽셀 기준)

        for (int i = 0; i < pieces; i++)
        {
            // spriteRect 범위 안에서 랜덤 좌표
            int w = (int)Random.Range(spriteRect.width / 6, spriteRect.width / 2);
            int h = (int)Random.Range(spriteRect.width / 6, spriteRect.width / 2);

            int x = (int)Random.Range(spriteRect.xMin,
                spriteRect.xMax - w);
            int y = (int)Random.Range(spriteRect.yMin,
                spriteRect.yMax - h);

            Color[] pix = tex.GetPixels(x, y, w, h);

            Texture2D pieceTex = new Texture2D(w, h);
            pieceTex.SetPixels(pix);
            pieceTex.Apply();

            Sprite pieceSprite = Sprite.Create(
                pieceTex,
                new Rect(0, 0, w, h),
                new Vector2(0.5f, 0.5f),
                sourceSprite.pixelsPerUnit
            );

            GameObject piece = new GameObject("Piece_" + i);
            piece.layer = LayerMask.NameToLayer("Broken");
            piece.transform.position = transform.position;
            var sr = piece.AddComponent<SpriteRenderer>();
            sr.sprite = pieceSprite;
            piece.transform.localScale = gameObject.transform.localScale;

            var rb = piece.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            rb.AddForce(Vector2.up * Random.Range(0.5f, 3f) + Vector2.right * Random.Range(-3f, 3f), ForceMode2D.Impulse);

            var col = piece.AddComponent<PolygonCollider2D>();
            Destroy(piece, 5f);
        }

        Destroy(gameObject);
    }
}