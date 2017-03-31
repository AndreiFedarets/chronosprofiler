// Chronos.TestConsole.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Timer.h"
#include "FastDictionary.h"
#include <Windows.h>
#include <iostream>
using namespace std;

void TestMap()
{
    cout << "===========Map Testing=============\r\n";
    __uint start;
    __uint time;
    __uint elapsed;
    __uint repeats = 1000000;
    //-----------------------------
    cout << "std::map>> ";
    std::map<UINT_PTR, UINT_PTR>* map = new std::map<UINT_PTR, UINT_PTR>();
    CFastDictionary<UINT_PTR>* dictionary = new  CFastDictionary<UINT_PTR>();
    for (UINT_PTR i = 0; i < 11111110; i += 1111111)
    {
        map->insert(std::pair<UINT_PTR, UINT_PTR>(i, i));
        dictionary->insert(i, i);
    }
    std::map<UINT_PTR, UINT_PTR>::iterator iter;
    UINT_PTR value;
    start = CTimer::CurrentTime;
    for (long i = 0; i < repeats; i++)
    {
        for (UINT_PTR j = 0; j < 11111110; j += 1111111)
        {
            UINT_PTR item  = map->find(i)->second;
            value += item;
            value -= item;
        }
    }
    elapsed = CTimer::CurrentTime - start;
    cout << elapsed;
    cout << "ms\r\n";
    //-----------------------------
    cout << "CTimer:CurrentTime>> ";
    start = CTimer::CurrentTime;
    for (long i = 0; i < repeats; i++)
    {
        for (UINT_PTR j = 0; j < 11111110; j += 1111111)
        {
            UINT_PTR item = dictionary->find(j);
            value += item;
            value -= item;
        }
    }
    elapsed = CTimer::CurrentTime - start;
    cout << elapsed;
    cout << "ms\r\n";
}

void TestTimer()
{
    cout << "===========Timer Testing=============\r\n";
    __uint start;
    __uint time;
    __uint elapsed;
    __uint repeats = 10000000;
    //-----------------------------
    cout << "GetTickCount>> ";
    start = CTimer::CurrentTime;
    for (long i = 0; i < repeats; i++)
    {
        time = GetTickCount();
    }
    elapsed = CTimer::CurrentTime - start;
    cout << elapsed;
    cout << "ms\r\n";
    //-----------------------------
    cout << "CTimer:CurrentTime>> ";
    start = CTimer::CurrentTime;
    for (long i = 0; i < repeats; i++)
    {
        time = CTimer::CurrentTime;
    }
    elapsed = CTimer::CurrentTime - start;
    cout << elapsed;
    cout << "ms\r\n";
}

int _tmain(int argc, _TCHAR* argv[])
{
    CTimer::Initialize();
    //for (int i = 0; i < 100; i++)
    //{
    //    __uint start = CTimer::GetTime();
    //    CTimer::SuspendThread(1);
    //    //Sleep(1);
    //    __uint elapsed =  CTimer::GetTime() - start;
    //    cout << elapsed;
    //    cout << "\e\n";
    //    Sleep(500);
    //}

    int a;
    cin >> a;
    /*CTimer::Initialize();
    TestMap();
    TestTimer();
    int result;
    cin >> result;
	return 0;*/
}

