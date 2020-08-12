# 中国省市区街道四级联动

## 目标

很多项目需要在填写地址中选省市区，本项目希望成为众多项目中的一个微服务~


## Web 框架选择

按：https://www.techempower.com/benchmarks 2020-05-28 的 Round 19 中

`Fortunes` 的权重比 `Plaintext` 高，因为 `Fortunes` 是一种更现实的测试类型，  
但本项目业务极其简单，基本上只是查询，一年中也不会有几次修改，所以选择 `Plaintext(代表响应性能)` 第一的 Asp.Net Core 框架~



## 数据来源

https://github.com/modood/Administrative-divisions-of-China

创建数据库，创建表 **district**

| 字段 | 类型                  | 是否是主键 | 注释         |
| ---- | --------------------- | ---------- | ------------ |
| id   | bigint NOT NULL       | 是         | 地区编码     |
| pid  | bigint NOT NULL       | 否         | 上级地区编码 |
| name | varchar(255) NOT NULL | 否         | 地区名称     |


``` sql
CREATE TABLE `district_cn`.`district`  (
  `id` bigint NOT NULL COMMENT '地区编码',
  `pid` bigint NOT NULL COMMENT '上级地区编码',
  `name` varchar(255) NOT NULL COMMENT '地区名称',
  PRIMARY KEY (`Id`)
);
```