#### 示例：新建一个最基本的关系

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";
}
```

这个类设置了最基本的几个属性，如显示颜色和ID以及和本关系对应的合法关系的ID。关于对应关系，后面的示例中会详细介绍。

你还需要给该关系创建对应的翻译文件，其中至少包含 `Example` 及 `ExampleLower` 两个键。它们分别对应 `Label` 和 `LabelCap`。

#### 示例：重写 `GetCustomShowValue` 函数以设置显示的值

你可以重写 `GetCustomShowValue` 函数以设置显示的值。

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";

    public override string GetCustomShowValue(Faction faction) => "20";
}
```

显示的值对应原版关系中好感度位置的文本。

#### 示例：重写 `GetCustomShowString` 函数以设置自定义显示文本

你可以重写 `GetCustomShowString` 函数以设置自定义显示文本。

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";

    public override string GetCustomShowString(Faction faction) => "custom text";
}
```

自定义显示文本是在原版关系中好感度位置下方添加的自定义文本。

#### 示例：重写 `GetCustomDetailString` 函数以设置鼠标悬浮时的详细文本

你可以重写 `GetCustomDetailString` 函数以设置鼠标悬浮时的详细文本。

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";

    public override string GetCustomDetailString(Faction faction) => "custom detail text";
}
```

如果你对以上这些绘制逻辑不满意，也可以重写 `PreDrawFactionRow` 和 `PostDrawFactionRow` 这两个函数来直接地修改绘制逻辑。

#### 示例：设置派系的关系

这里我们以玩家的关系举例：

当你在自定义的逻辑中判定一个派系已经满足了某一关系的条件，你就可以使用如下代码：

```csharp
FactionRelationUtils.GetCustomFactionRelationKind<ExampleRelation>().SetRelation(faction.RelationWith(Faction.OfPlayer))
```

当然，你同样可以设置派系与派系之间的关系，它们的逻辑是相同的。

#### 示例：对应关系

如果你想创建两个对应的关系，而非相同的对应关系，你可以像如下这样：

```csharp
public class ExampleRelation1 : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example1";

    public override string LegalAnotherFactionRelationKindID => "Example2";
}

public class ExampleRelation2 : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example2";

    public override string LegalAnotherFactionRelationKindID => "Example1";
}
```

这样，在你设置对应派系与你的派系关系为 `Example1` 时，会自动设置你的派系与对应派系关系为 `Example2`。这可以用于不对等的关系，如宗主派系与附庸派系。

当前版本中，一个派系对应多个派系的操作暂不支持，请期待以后的更新。