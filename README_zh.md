# pp-server



### 接口

```
GET /pp
```

### 参数

```
bid             BeatmapId
mode            游戏模式： 0 = osu!, 1 = taiko, 3 = mania(不支持ctb)
mods            模式简称组合. 例如: HDDT or DTHR or HRHD
acc             设置一个acc的百分比. 取值范围: 0-100
miss            miss的数量
combo           连击的数量
good            100的数量
meh             50的数量
score           分数
percentCombo    设置一个百分比，自动计算最大连击数，范围0-100
```



### 示例

![image-20200518142328659](https://raw.githubusercontent.com/wanjiaXG/pp-server/master/image/osu.png)



![image-20200518142328659](https://raw.githubusercontent.com/wanjiaXG/pp-server/master/image/mania.png)