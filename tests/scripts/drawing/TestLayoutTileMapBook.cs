using GdUnit4;
using MPewsey.ManiaMap;
using MPewsey.ManiaMap.Samples;
using MPewsey.ManiaMapGodot.Tests;

namespace MPewsey.ManiaMapGodot.Drawing.Tests
{
    [TestSuite]
    public class TestLayoutTileMapBook
    {
        private const string LayoutTileMapBookScene = "uid://npua7l2tbnn0";

        [TestCase]
        public void TestDrawMap()
        {
            var runner = SceneRunner.RunScene(LayoutTileMapBookScene);
            var map = runner.Scene() as LayoutTileMapBook;
            Assertions.AssertThat(map != null).IsTrue();

            var results = BigLayoutSample.Generate(12345);
            Assertions.AssertThat(results.Success).IsTrue();
            var layout = results.GetOutput<Layout>("Layout");
            var layoutState = new LayoutState(layout);

            ManiaMapManager.Initialize(layout, layoutState);
            map.DrawPages();
        }
    }
}
