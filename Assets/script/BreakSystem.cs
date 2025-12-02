using UnityEngine;

public class BreakSystem : MonoBehaviour
{
    public Sprite sourceSprite;
    public int pieces = 6;
    
    public AudioClip breakSound;
    public float soundVolume = 1f;

    private void Start()
    {
        sourceSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void Break()
    {
        if (breakSound != null)
            AudioSource.PlayClipAtPoint(breakSound, transform.position, soundVolume);

        if (sourceSprite == null)
            sourceSprite = GetComponent<SpriteRenderer>().sprite;

        Texture2D tex = sourceSprite.texture;
        Rect spriteRect = sourceSprite.rect;

        for (int i = 0; i < pieces; i++)
        {
            int w = (int)Random.Range(spriteRect.width / 6, spriteRect.width / 2);
            int h = (int)Random.Range(spriteRect.width / 6, spriteRect.width / 2);

            int x = (int)Random.Range(spriteRect.xMin, spriteRect.xMax - w);
            int y = (int)Random.Range(spriteRect.yMin, spriteRect.yMax - h);

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
            rb.AddForce(Vector2.up * Random.Range(0.5f, 3f)
                        + Vector2.right * Random.Range(-3f, 3f),
                        ForceMode2D.Impulse);

            var col = piece.AddComponent<PolygonCollider2D>();
            Destroy(piece, 5f);
        }

        Destroy(gameObject);
    }
}
