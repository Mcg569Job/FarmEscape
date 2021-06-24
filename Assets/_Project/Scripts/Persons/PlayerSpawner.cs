public class PlayerSpawner : PersonCreator
{
    private void Start()
    {
        personCount = Data.Get.LevelData.GetCurrentlevel().PlayerCount;
        CreatePersons();
    }
}
