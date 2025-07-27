
/*
                            << Item >>

        - 아이템 Root 클래스
            - ItemData 데이터로 초기화
 */

public abstract class Item
{
    public ItemData Data { get; private set; }

    public Item(ItemData data) => Data = data;
}
