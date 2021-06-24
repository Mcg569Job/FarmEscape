public class EnemySpawner : PersonCreator
{
    private void Start()
    {
        personCount = Data.Get.LevelData.GetCurrentlevel().EnemyCount;
        CreatePersons();
    }

}
