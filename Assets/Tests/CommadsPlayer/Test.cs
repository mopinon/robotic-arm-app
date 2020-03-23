using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.CommadsPlayer
{
    public class Test
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestGetTargetAngelsSimplePasses()
        {
            
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestGetCommandsWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
