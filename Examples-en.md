#### Example: Creating a Basic Relationship

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";
}
```

This class sets the most basic properties, such as the display color, ID, and the ID of the legal relationship corresponding to this relationship.

You also need to create a translation file for this relationship, which must include at least the keys `Example` and `ExampleLower`. These correspond to `Label` and `LabelCap`, respectively.

#### Example: Overriding `GetCustomShowValue` Method to Set the Displayed Value

You can override `GetCustomShowValue` method to set the displayed value.

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";

    public override string GetCustomShowValue(Faction faction) => "20";
}
```

The displayed value corresponds to the text in the goodwill position in the vanilla relationship.

#### Example: Overriding `GetCustomShowString` Method to Set Custom Display Text

You can override `GetCustomShowString` method to set custom display text.

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";

    public override string GetCustomShowString(Faction faction) => "custom text";
}
```

The custom display text is added below the goodwill position in the vanilla relationship.

#### Example: Overriding `GetCustomDetailString` Method to Set Detailed Text on Mouse Hover

You can override the `GetCustomDetailString` function to set detailed text displayed on mouse hover.

```csharp
public class ExampleRelation : CustomFactionRelationKind
{
    public override Color GetColor() => Color.yellow;

    public override string ID => "Example";

    public override string LegalAnotherFactionRelationKindID => "Example";

    public override string GetCustomDetailString(Faction faction) => "custom detail text";
}
```

If you are not satisfied with these drawing logics, you can also override the `PreDrawFactionRow` and `PostDrawFactionRow` methods to directly modify the drawing logic.

#### Example: Setting Faction Relationships

Let's take the player's relationship as an example:

When you determine in your custom logic that a faction has met the conditions for a certain relationship, you can use the following code:

```csharp
FactionRelationUtils.GetCustomFactionRelationKind<ExampleRelation>().SetRelation(faction.RelationWith(Faction.OfPlayer))
```

Of course, you can also set relationships between factions, and the logic is the same.

#### Example: Corresponding Relationships

If you want to create two corresponding relationships instead of identical ones, you can do it like this:

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

This way, when you set the relationship of a corresponding faction with your faction to `Example1`, it will automatically set your faction's relationship with the corresponding faction to `Example2`. This can be used for asymmetric relationships, such as suzerain factions and vassal factions.

In the current version, operations involving one faction corresponding to multiple factions are not yet supported. Please look forward to future updates.