using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTimeStap
{
    public int year;
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
    public enum DayOfTheWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
    public Season season;
    public int day;
    public int hour;
    public int minute;
    public GameTimeStap(int year, Season season, int day, int hour, int minute)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minute = minute;
    }
    public GameTimeStap(GameTimeStap timeStamp)
    {
        this.year=timeStamp.year;
        this.season = timeStamp.season;
        this.day = timeStamp.day;
        this.hour = timeStamp.hour;
        this.minute = timeStamp.minute;
    }
    public void UpdateClock()
    {
        minute++;
        if(minute>= 60)
        {
            minute = 0;
            hour++;
        }
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }
        if(day > 30)
        {
            day = 1;
            if(season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }
    }
    public DayOfTheWeek GetDayOfTheWeek()
    {
        int dayPassed = YearsToDays(year) + SeasonToDays(season) + day;
        int dayIndex = dayPassed % 7;

        return (DayOfTheWeek)dayIndex;
    }
    public static int HourToMinute(int hour)
    {
        return hour * 60;
    }
    public static int DayToHour(int day)
    {
        return day * 24;
    }
    public static int SeasonToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }
    public static int YearsToDays(int years)
    {
        return years * 4 * 30;
    }
    public static int TimestampInMinutes(GameTimeStap timestamp)
    {
        return HourToMinute(DayToHour(YearsToDays(timestamp.year)) + DayToHour(SeasonToDays(timestamp.season)) + DayToHour(timestamp.day) + timestamp.hour) + timestamp.minute;
    }
    public static int CompareTimeStamps(GameTimeStap timestamp1,GameTimeStap timestamp2)
    {
        int timestamp1Hours = DayToHour(YearsToDays(timestamp1.year))+ DayToHour(SeasonToDays(timestamp1.season))+DayToHour(timestamp1.day)+timestamp1.hour;
        int timestamp2Hours = DayToHour(YearsToDays(timestamp2.year)) + DayToHour(SeasonToDays(timestamp2.season)) + DayToHour(timestamp2.day) + timestamp2.hour;
        int difference = timestamp2Hours - timestamp1Hours;
        return Mathf.Abs(difference);
    }
}
