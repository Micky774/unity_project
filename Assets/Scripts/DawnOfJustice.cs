using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

public class DawnOfJustice : MonoBehaviour {
    private const int NUM_ITERATIONS = 10000000;
    private int int0, int1, int2, int3, int4, int5, int6, int7, int8, int9, int10, int11, int12, int13, int14, int15, int16, int17, int18, int19;
    private bool straight_boolin;

    private enum Enum3 {
        enum0 = 0,
        enum1 = 1,
        enum2 = 2
    }

    private enum Enum5 {
        enum0 = 0,
        enum1 = 1,
        enum2 = 2,
        enum3 = 3,
        enum4 = 4
    }

    private enum Enum8 {
        enum0 = 0,
        enum1 = 1,
        enum2 = 2,
        enum3 = 3,
        enum4 = 4,
        enum5 = 5,
        enum6 = 6,
        enum7 = 7
    }

    private enum Enum12 {
        enum0 = 0,
        enum1 = 1,
        enum2 = 2,
        enum3 = 3,
        enum4 = 4,
        enum5 = 5,
        enum6 = 6,
        enum7 = 7,
        enum8 = 8,
        enum9 = 9,
        enum10 = 10,
        enum11 = 11
    }

    private enum Enum20 {
        enum0 = 0,
        enum1 = 1,
        enum2 = 2,
        enum3 = 3,
        enum4 = 4,
        enum5 = 5,
        enum6 = 6,
        enum7 = 7,
        enum8 = 8,
        enum9 = 9,
        enum10 = 10,
        enum11 = 11,
        enum12 = 12,
        enum13 = 13,
        enum14 = 14,
        enum15 = 15,
        enum16 = 16,
        enum17 = 17,
        enum18 = 18,
        enum19 = 19
    }

    protected void Start() {
        Comparatron9000(NUM_ITERATIONS);
    }

    protected void Comparatron9000(int num_iterations) {
        Stopwatch stopwatch = new Stopwatch();

        Debug.Log("Enum length 3:");
        Array enum3array = Enum.GetValues(typeof(Enum3));
        Enum3 randChoice3;

        ClearInts();
        Func<bool>[] array3 = new Func<bool>[] { func0, func1, func2 };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice3 = (Enum3)enum3array.GetValue(Random.Range(0, 3));
            straight_boolin = array3[(int)randChoice3]();
        }
        stopwatch.Stop();
        Debug.Log("Array strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice3 = (Enum3)enum3array.GetValue(Random.Range(0, 3));
            switch(randChoice3) {
                case Enum3.enum0:
                    straight_boolin = func0();
                    break;
                case Enum3.enum1:
                    straight_boolin = func1();
                    break;
                case Enum3.enum2:
                    straight_boolin = func2();
                    break;
                default:
                    throw new Exception("That wasn't supposed to happen.");
            }
        }
        stopwatch.Stop();
        Debug.Log("Switch strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        IDictionary<Enum3, Func<bool>> dict3 = new Dictionary<Enum3, Func<bool>>(){
            {Enum3.enum0, func0},
            {Enum3.enum1, func1},
            {Enum3.enum2, func2}
        };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice3 = (Enum3)enum3array.GetValue(Random.Range(0, 3));
            straight_boolin = dict3[randChoice3]();
        }
        stopwatch.Stop();
        Debug.Log("Dictionary strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        Debug.Log("");
        Debug.Log("Enum length 5:");
        Array enum5array = Enum.GetValues(typeof(Enum5));
        Enum5 randChoice5;

        ClearInts();
        Func<bool>[] array5 = new Func<bool>[] { func0, func1, func2, func3, func4 };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice5 = (Enum5)enum5array.GetValue(Random.Range(0, 5));
            straight_boolin = array5[(int)randChoice5]();
        }
        stopwatch.Stop();
        Debug.Log("Array strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice5 = (Enum5)enum5array.GetValue(Random.Range(0, 5));
            switch(randChoice5) {
                case Enum5.enum0:
                    straight_boolin = func0();
                    break;
                case Enum5.enum1:
                    straight_boolin = func1();
                    break;
                case Enum5.enum2:
                    straight_boolin = func2();
                    break;
                case Enum5.enum3:
                    straight_boolin = func3();
                    break;
                case Enum5.enum4:
                    straight_boolin = func4();
                    break;
                default:
                    throw new Exception("That wasn't supposed to happen.");
            }
        }
        stopwatch.Stop();
        Debug.Log("Switch strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        IDictionary<Enum5, Func<bool>> dict5 = new Dictionary<Enum5, Func<bool>>(){
            {Enum5.enum0, func0},
            {Enum5.enum1, func1},
            {Enum5.enum2, func2},
            {Enum5.enum3, func3},
            {Enum5.enum4, func4}
        };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice5 = (Enum5)enum5array.GetValue(Random.Range(0, 5));
            straight_boolin = dict5[randChoice5]();
        }
        stopwatch.Stop();
        Debug.Log("Dictionary strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        Debug.Log("");
        Debug.Log("Enum length 8:");
        Array enum8array = Enum.GetValues(typeof(Enum8));
        Enum8 randChoice8;

        ClearInts();
        Func<bool>[] array8 = new Func<bool>[] { func0, func1, func2, func3, func4, func5, func6, func7 };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice8 = (Enum8)enum8array.GetValue(Random.Range(0, 8));
            straight_boolin = array8[(int)randChoice8]();
        }
        stopwatch.Stop();
        Debug.Log("Array strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice8 = (Enum8)enum8array.GetValue(Random.Range(0, 8));
            switch(randChoice8) {
                case Enum8.enum0:
                    straight_boolin = func0();
                    break;
                case Enum8.enum1:
                    straight_boolin = func1();
                    break;
                case Enum8.enum2:
                    straight_boolin = func2();
                    break;
                case Enum8.enum3:
                    straight_boolin = func3();
                    break;
                case Enum8.enum4:
                    straight_boolin = func4();
                    break;
                case Enum8.enum5:
                    straight_boolin = func5();
                    break;
                case Enum8.enum6:
                    straight_boolin = func6();
                    break;
                case Enum8.enum7:
                    straight_boolin = func7();
                    break;
                default:
                    throw new Exception("That wasn't supposed to happen.");
            }
        }
        stopwatch.Stop();
        Debug.Log("Switch strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        IDictionary<Enum8, Func<bool>> dict8 = new Dictionary<Enum8, Func<bool>>(){
            {Enum8.enum0, func0},
            {Enum8.enum1, func1},
            {Enum8.enum2, func2},
            {Enum8.enum3, func3},
            {Enum8.enum4, func4},
            {Enum8.enum5, func5},
            {Enum8.enum6, func6},
            {Enum8.enum7, func7}
        };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice8 = (Enum8)enum8array.GetValue(Random.Range(0, 8));
            straight_boolin = dict8[randChoice8]();
        }
        stopwatch.Stop();
        Debug.Log("Dictionary strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        Debug.Log("");
        Debug.Log("Enum length 12:");
        Array enum12array = Enum.GetValues(typeof(Enum12));
        Enum12 randChoice12;

        ClearInts();
        Func<bool>[] array12 = new Func<bool>[] { func0, func1, func2, func3, func4, func5, func6, func7, func8, func9, func10, func11 };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice12 = (Enum12)enum12array.GetValue(Random.Range(0, 12));
            straight_boolin = array12[(int)randChoice12]();
        }
        stopwatch.Stop();
        Debug.Log("Array strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice12 = (Enum12)enum12array.GetValue(Random.Range(0, 12));
            switch(randChoice12) {
                case Enum12.enum0:
                    straight_boolin = func0();
                    break;
                case Enum12.enum1:
                    straight_boolin = func1();
                    break;
                case Enum12.enum2:
                    straight_boolin = func2();
                    break;
                case Enum12.enum3:
                    straight_boolin = func3();
                    break;
                case Enum12.enum4:
                    straight_boolin = func4();
                    break;
                case Enum12.enum5:
                    straight_boolin = func5();
                    break;
                case Enum12.enum6:
                    straight_boolin = func6();
                    break;
                case Enum12.enum7:
                    straight_boolin = func7();
                    break;
                case Enum12.enum8:
                    straight_boolin = func8();
                    break;
                case Enum12.enum9:
                    straight_boolin = func9();
                    break;
                case Enum12.enum10:
                    straight_boolin = func10();
                    break;
                case Enum12.enum11:
                    straight_boolin = func11();
                    break;
                default:
                    throw new Exception("That wasn't supposed to happen.");
            }
        }
        stopwatch.Stop();
        Debug.Log("Switch strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        IDictionary<Enum12, Func<bool>> dict12 = new Dictionary<Enum12, Func<bool>>(){
            {Enum12.enum0, func0},
            {Enum12.enum1, func1},
            {Enum12.enum2, func2},
            {Enum12.enum3, func3},
            {Enum12.enum4, func4},
            {Enum12.enum5, func5},
            {Enum12.enum6, func6},
            {Enum12.enum7, func7},
            {Enum12.enum8, func8},
            {Enum12.enum9, func9},
            {Enum12.enum10, func10},
            {Enum12.enum11, func11}
        };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice12 = (Enum12)enum12array.GetValue(Random.Range(0, 12));
            straight_boolin = dict12[randChoice12]();
        }
        stopwatch.Stop();
        Debug.Log("Dictionary strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        Debug.Log("");
        Debug.Log("Enum length 20:");
        Array enum20array = Enum.GetValues(typeof(Enum20));
        Enum20 randChoice20;

        ClearInts();
        Func<bool>[] array20 = new Func<bool>[] { func0, func1, func2, func3, func4, func5, func6, func7, func8, func9, func10, func11, func12, func13, func14, func15, func16, func17, func18, func19 };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice20 = (Enum20)enum20array.GetValue(Random.Range(0, 20));
            straight_boolin = array20[(int)randChoice20]();
        }
        stopwatch.Stop();
        Debug.Log("Array strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice20 = (Enum20)enum20array.GetValue(Random.Range(0, 20));
            switch(randChoice20) {
                case Enum20.enum0:
                    straight_boolin = func0();
                    break;
                case Enum20.enum1:
                    straight_boolin = func1();
                    break;
                case Enum20.enum2:
                    straight_boolin = func2();
                    break;
                case Enum20.enum3:
                    straight_boolin = func3();
                    break;
                case Enum20.enum4:
                    straight_boolin = func4();
                    break;
                case Enum20.enum5:
                    straight_boolin = func5();
                    break;
                case Enum20.enum6:
                    straight_boolin = func6();
                    break;
                case Enum20.enum7:
                    straight_boolin = func7();
                    break;
                case Enum20.enum8:
                    straight_boolin = func8();
                    break;
                case Enum20.enum9:
                    straight_boolin = func9();
                    break;
                case Enum20.enum10:
                    straight_boolin = func10();
                    break;
                case Enum20.enum11:
                    straight_boolin = func11();
                    break;
                case Enum20.enum12:
                    straight_boolin = func12();
                    break;
                case Enum20.enum13:
                    straight_boolin = func13();
                    break;
                case Enum20.enum14:
                    straight_boolin = func14();
                    break;
                case Enum20.enum15:
                    straight_boolin = func15();
                    break;
                case Enum20.enum16:
                    straight_boolin = func16();
                    break;
                case Enum20.enum17:
                    straight_boolin = func17();
                    break;
                case Enum20.enum18:
                    straight_boolin = func18();
                    break;
                case Enum20.enum19:
                    straight_boolin = func19();
                    break;
                default:
                    throw new Exception("That wasn't supposed to happen.");
            }
        }
        stopwatch.Stop();
        Debug.Log("Switch strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();

        ClearInts();
        IDictionary<Enum20, Func<bool>> dict20 = new Dictionary<Enum20, Func<bool>>(){
            {Enum20.enum0, func0},
            {Enum20.enum1, func1},
            {Enum20.enum2, func2},
            {Enum20.enum3, func3},
            {Enum20.enum4, func4},
            {Enum20.enum5, func5},
            {Enum20.enum6, func6},
            {Enum20.enum7, func7},
            {Enum20.enum8, func8},
            {Enum20.enum9, func9},
            {Enum20.enum10, func10},
            {Enum20.enum11, func11},
            {Enum20.enum12, func12},
            {Enum20.enum13, func13},
            {Enum20.enum14, func14},
            {Enum20.enum15, func15},
            {Enum20.enum16, func16},
            {Enum20.enum17, func17},
            {Enum20.enum18, func18},
            {Enum20.enum19, func19}
        };
        stopwatch.Start();
        for(int i = 0; i < num_iterations; i++) {
            randChoice20 = (Enum20)enum20array.GetValue(Random.Range(0, 20));
            straight_boolin = dict20[randChoice20]();
        }
        stopwatch.Stop();
        Debug.Log("Dictionary strat with " + num_iterations + " iterations: " + stopwatch.Elapsed + " seconds");
        stopwatch.Reset();
    }

    private void ClearInts() {
        int0 = 0;
        int1 = 0;
        int2 = 0;
        int3 = 0;
        int4 = 0;
        int5 = 0;
        int6 = 0;
        int7 = 0;
        int8 = 0;
        int9 = 0;
        int10 = 0;
        int11 = 0;
        int12 = 0;
        int13 = 0;
        int14 = 0;
        int15 = 0;
        int16 = 0;
        int17 = 0;
        int18 = 0;
        int19 = 0;
    }

    private bool func0() {
        int0 += 1;
        return true;
    }
    private bool func1() {
        int1 += 1;
        return true;
    }
    private bool func2() {
        int2 += 1;
        return true;
    }
    private bool func3() {
        int3 += 1;
        return true;
    }
    private bool func4() {
        int4 += 1;
        return true;
    }
    private bool func5() {
        int5 += 1;
        return true;
    }
    private bool func6() {
        int6 += 1;
        return true;
    }
    private bool func7() {
        int7 += 1;
        return true;
    }
    private bool func8() {
        int8 += 1;
        return true;
    }
    private bool func9() {
        int9 += 1;
        return true;
    }
    private bool func10() {
        int10 += 1;
        return true;
    }
    private bool func11() {
        int11 += 1;
        return true;
    }
    private bool func12() {
        int12 += 1;
        return true;
    }
    private bool func13() {
        int13 += 1;
        return true;
    }
    private bool func14() {
        int14 += 1;
        return true;
    }
    private bool func15() {
        int15 += 1;
        return true;
    }
    private bool func16() {
        int16 += 1;
        return true;
    }
    private bool func17() {
        int17 += 1;
        return true;
    }
    private bool func18() {
        int18 += 1;
        return true;
    }
    private bool func19() {
        int19 += 1;
        return true;
    }
}
