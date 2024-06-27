using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    public List<Prop> props;

    public void PlaceProps(ItemPlacementHelper itemPlacementHelper)
    {
        foreach (var prop in props)
        {
            PlaceProp(prop, itemPlacementHelper);
        }
    }

    private void PlaceProp(Prop prop, ItemPlacementHelper itemPlacementHelper)
    {
        int quantity = Random.Range(prop.PlacementQuantityMin, prop.PlacementQuantityMax + 1);
        for (int i = 0; i < quantity; i++)
        {
            Vector2Int? position = null;
            if (prop.Corner) position = ConvertToVector2Int(itemPlacementHelper.GetItemPlacementPosition(PlacementType.NearWall, 100, prop.PropSize, false));
            if (!position.HasValue && prop.NearWallUP) position = ConvertToVector2Int(itemPlacementHelper.GetItemPlacementPosition(PlacementType.NearWall, 100, prop.PropSize, false));
            if (!position.HasValue && prop.NearWallDown) position = ConvertToVector2Int(itemPlacementHelper.GetItemPlacementPosition(PlacementType.NearWall, 100, prop.PropSize, false));
            if (!position.HasValue && prop.NearWallRight) position = ConvertToVector2Int(itemPlacementHelper.GetItemPlacementPosition(PlacementType.NearWall, 100, prop.PropSize, false));
            if (!position.HasValue && prop.NearWallLeft) position = ConvertToVector2Int(itemPlacementHelper.GetItemPlacementPosition(PlacementType.NearWall, 100, prop.PropSize, false));
            if (!position.HasValue && prop.Inner) position = ConvertToVector2Int(itemPlacementHelper.GetItemPlacementPosition(PlacementType.OpenSpace, 100, prop.PropSize, false));

            if (position.HasValue)
            {
                CreateProp(prop, position.Value);
            }
        }
    }

    private Vector2Int? ConvertToVector2Int(Vector2? vector)
    {
        if (vector.HasValue)
        {
            return new Vector2Int(Mathf.RoundToInt(vector.Value.x), Mathf.RoundToInt(vector.Value.y));
        }
        return null;
    }

    private void CreateProp(Prop prop, Vector2Int position)
    {
        GameObject propObject = new GameObject(prop.name);
        propObject.transform.position = new Vector3(position.x, position.y, 0);

        SpriteRenderer spriteRenderer = propObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = prop.PropSprite;

        // Optional: Add a collider based on prop size
        BoxCollider2D collider = propObject.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(prop.PropSize.x, prop.PropSize.y);
    }
}
