# pp-server



### URL

```
GET /pp
```

### Parameters

```
bid             BeatmapId
mode            game mode： 0 = osu!, 1 = taiko, 3 = mania(Not support ctb)
mods            Use short name. Example: HDDT or DTHR or HRHD
acc             Percentage of acc 0-100
miss            The number of misses
combo           The number of combo
good            The number of good(100)
meh             The number of meh(50)
score           The number of score
percentCombo    Set a percentage to automatically calculate the number of combos, combo = percentCombo * maxcombo
```



### Examples

![image-20200518142328659](https://raw.githubusercontent.com/wanjiaXG/pp-server/master/image/osu.png)



![image-20200518142328659](https://raw.githubusercontent.com/wanjiaXG/pp-server/master/image/mania.png)