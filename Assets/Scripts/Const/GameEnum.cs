/// <summary>
/// 枪械类型枚举
/// </summary>
public enum GunType
{
	AssaultRifle,
	HuntingRifle,
	Revolver,
	ShotGun,
	WoodenBow,
	WoodenSpear
}


/// <summary>
/// 材质类型枚举
/// </summary>
public enum MaterialType
{
	Stone,		// 石头
	Metal,		// 金属
	Wood		// 木头
}


/// <summary>
///  代表 AI 状态的枚举
/// </summary>
public enum AIState
{
	IDLE,           // 由于 idle 就相当于NULL 因此不再对该枚举设置 NULL 枚举项
	WALK,
	RUN,
	ATTACK,
	DEATH
}



/// <summary>
///  AI控制器的类型
/// </summary>
public enum AIManagerType
{
	CANNIBAL,
	BOAR,
	NULL                    // 枚举的细节，给新枚举赋值
}
