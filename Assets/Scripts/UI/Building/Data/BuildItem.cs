
public class BuildItem  
{
    private string name;

    private string posX;
    private string posY;
    private string posZ;


    private string rotX;
    private string rotY;
    private string rotZ;
    private string rotW;

    public string Name { get { return name; } set { name = value; } }
    public string PosX { get { return posX; } set { posX = value; } }
    public string PosY { get { return posY; } set { posY = value; } }
    public string PosZ { get { return posZ; } set { posZ = value; } }
    public string RotX { get { return rotX; } set { rotX = value; } }
    public string RotY { get { return rotY; } set { rotY = value; } }
    public string RotZ { get { return rotZ; } set { rotZ = value; } }
    public string RotW { get { return rotW; } set { rotW = value; } }


    public BuildItem() { }

    public BuildItem(string name,string posX,string posY,string posZ,string rotX,string rotY,string rotZ,string rotW)
    {
        this.name = name;
        this.posX = posX;
        this.posY = posY;
        this.posZ = posZ;
        this.rotX = rotX;
        this.rotY = rotY;
        this.rotZ = rotZ;
        this.rotW = rotW;
    }

}
