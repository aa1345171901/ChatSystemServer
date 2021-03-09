/*
Navicat MySQL Data Transfer

Source Server         : localhost_3306
Source Server Version : 50726
Source Host           : localhost:3306
Source Database       : chat_system

Target Server Type    : MYSQL
Target Server Version : 50726
File Encoding         : 65001

Date: 2021-03-06 20:52:37
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for bloodtype
-- ----------------------------
DROP TABLE IF EXISTS `bloodtype`;
CREATE TABLE `bloodtype` (
  `id` int(11) NOT NULL,
  `BloodType` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `BloodType` (`BloodType`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of bloodtype
-- ----------------------------
INSERT INTO `bloodtype` VALUES ('-1', null);
INSERT INTO `bloodtype` VALUES ('0', null);
INSERT INTO `bloodtype` VALUES ('4', 'AB型血');
INSERT INTO `bloodtype` VALUES ('1', 'A型血');
INSERT INTO `bloodtype` VALUES ('2', 'B型血');
INSERT INTO `bloodtype` VALUES ('3', 'O型血');

-- ----------------------------
-- Table structure for face
-- ----------------------------
DROP TABLE IF EXISTS `face`;
CREATE TABLE `face` (
  `id` int(12) NOT NULL AUTO_INCREMENT,
  `path` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of face
-- ----------------------------
INSERT INTO `face` VALUES ('1', null);
INSERT INTO `face` VALUES ('104', null);

-- ----------------------------
-- Table structure for friend
-- ----------------------------
DROP TABLE IF EXISTS `friend`;
CREATE TABLE `friend` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `hostFriendId` int(12) DEFAULT NULL,
  `accetFriendId` int(12) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=32 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of friend
-- ----------------------------
INSERT INTO `friend` VALUES ('9', '11', '22');
INSERT INTO `friend` VALUES ('10', '22', '11');
INSERT INTO `friend` VALUES ('11', '43', '42');
INSERT INTO `friend` VALUES ('12', '22', '1');
INSERT INTO `friend` VALUES ('13', '1', '22');
INSERT INTO `friend` VALUES ('29', '51', '52');
INSERT INTO `friend` VALUES ('30', '52', '51');

-- ----------------------------
-- Table structure for messages
-- ----------------------------
DROP TABLE IF EXISTS `messages`;
CREATE TABLE `messages` (
  `msgId` int(11) NOT NULL AUTO_INCREMENT,
  `touserid` int(12) DEFAULT NULL,
  `fromuserid` int(12) DEFAULT NULL,
  `message` text,
  `messagetype` int(1) DEFAULT NULL,
  `messagestate` int(1) DEFAULT NULL,
  `havePlayAdiuo` int(1) DEFAULT '0',
  `sendtime` datetime DEFAULT NULL,
  PRIMARY KEY (`msgId`)
) ENGINE=InnoDB AUTO_INCREMENT=59 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of messages
-- ----------------------------
INSERT INTO `messages` VALUES ('1', '22', '1', '你好鸭', '1', '1', '1', '2019-09-08 14:20:07');
INSERT INTO `messages` VALUES ('2', '22', '11', null, '2', '1', '1', '2019-09-08 14:20:11');
INSERT INTO `messages` VALUES ('32', '1', '22', '你好', '1', '1', '1', '2019-09-09 16:27:20');
INSERT INTO `messages` VALUES ('33', '22', '1', '你好', '1', '1', '1', '2019-09-09 16:27:50');
INSERT INTO `messages` VALUES ('34', '22', '1', '你好啊', '1', '1', '1', '2019-09-16 22:21:15');
INSERT INTO `messages` VALUES ('35', '1', '22', '你好啊！Nick', '1', '1', '1', '2019-09-16 22:21:53');
INSERT INTO `messages` VALUES ('37', '1', '1', null, '2', '1', '1', null);
INSERT INTO `messages` VALUES ('38', '22', '1', '123', '1', '0', '0', '2021-02-09 15:12:09');
INSERT INTO `messages` VALUES ('39', '22', '1', '123', '1', '0', '0', '2021-02-09 15:12:12');
INSERT INTO `messages` VALUES ('43', '52', '51', null, '2', '1', '1', '2021-02-10 16:50:52');
INSERT INTO `messages` VALUES ('45', '50', '51', null, '2', '0', '0', '2021-02-10 17:10:04');
INSERT INTO `messages` VALUES ('46', '51', '52', '你好\r\n', '1', '1', '1', '2021-02-14 17:56:30');
INSERT INTO `messages` VALUES ('47', '52', '51', 'hello', '1', '1', '1', '2021-02-14 17:57:00');
INSERT INTO `messages` VALUES ('48', '51', '52', 'nih\r\n', '1', '1', '1', '2021-02-14 18:21:25');
INSERT INTO `messages` VALUES ('49', '52', '50', '123', '1', '1', '1', '2021-02-15 15:13:46');
INSERT INTO `messages` VALUES ('50', '51', '52', '123', '1', '1', '1', '2021-02-15 16:53:05');
INSERT INTO `messages` VALUES ('51', '51', '52', '123', '1', '1', '1', '2021-02-15 16:53:08');
INSERT INTO `messages` VALUES ('52', '51', '52', '123', '1', '1', '1', '2021-02-15 16:53:08');
INSERT INTO `messages` VALUES ('53', '51', '52', '123', '1', '1', '1', '2021-02-15 16:53:08');
INSERT INTO `messages` VALUES ('54', '51', '52', '123', '1', '1', '1', '2021-02-15 16:53:08');
INSERT INTO `messages` VALUES ('55', '51', '52', '123', '1', '1', '1', '2021-02-15 16:53:11');
INSERT INTO `messages` VALUES ('56', '51', '52', '123', '1', '1', '1', '2021-02-15 16:54:33');
INSERT INTO `messages` VALUES ('57', '51', '52', '123', '1', '1', '1', '2021-02-15 17:02:13');
INSERT INTO `messages` VALUES ('58', '51', '52', 'fsda', '1', '0', '0', '2021-03-06 16:43:22');

-- ----------------------------
-- Table structure for start
-- ----------------------------
DROP TABLE IF EXISTS `start`;
CREATE TABLE `start` (
  `id` int(11) NOT NULL,
  `start` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `start` (`start`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of start
-- ----------------------------
INSERT INTO `start` VALUES ('-1', null);
INSERT INTO `start` VALUES ('0', null);
INSERT INTO `start` VALUES ('3', '双子座（5.21-6.21）');
INSERT INTO `start` VALUES ('12', '双鱼座（2.19-3.20）');
INSERT INTO `start` VALUES ('6', '处女座（8.23-9.22）');
INSERT INTO `start` VALUES ('7', '天平座（9.23-10.23）');
INSERT INTO `start` VALUES ('8', '天蝎座（10.24-11.22）');
INSERT INTO `start` VALUES ('9', '射手座（11.23-12.21）');
INSERT INTO `start` VALUES ('4', '巨蟹座（6.22-7.22）');
INSERT INTO `start` VALUES ('10', '摩羯座（12.22-1.19）');
INSERT INTO `start` VALUES ('11', '水瓶座（1.20-2.18）');
INSERT INTO `start` VALUES ('5', '狮子座（7.23-8.22）');
INSERT INTO `start` VALUES ('1', '白羊座（3.21-4.19）');
INSERT INTO `start` VALUES ('2', '金牛座（4.20-5.20）');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` int(12) NOT NULL AUTO_INCREMENT,
  `password` varchar(20) NOT NULL,
  `dataid` int(10) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_data` (`dataid`),
  CONSTRAINT `user_data` FOREIGN KEY (`dataid`) REFERENCES `userdata` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('1', '1', '1');
INSERT INTO `user` VALUES ('11', '123', '8');
INSERT INTO `user` VALUES ('12', '1', '9');
INSERT INTO `user` VALUES ('13', '1', '10');
INSERT INTO `user` VALUES ('14', '1', '11');
INSERT INTO `user` VALUES ('15', '1', '12');
INSERT INTO `user` VALUES ('16', '1', '13');
INSERT INTO `user` VALUES ('17', '1', '14');
INSERT INTO `user` VALUES ('18', '1', '15');
INSERT INTO `user` VALUES ('19', '1', '16');
INSERT INTO `user` VALUES ('20', '1', '17');
INSERT INTO `user` VALUES ('21', '1', '18');
INSERT INTO `user` VALUES ('22', '1', '19');
INSERT INTO `user` VALUES ('23', '1', '20');
INSERT INTO `user` VALUES ('24', '11', '21');
INSERT INTO `user` VALUES ('27', '1', '24');
INSERT INTO `user` VALUES ('28', '1', '25');
INSERT INTO `user` VALUES ('29', '1', '26');
INSERT INTO `user` VALUES ('30', '1', '27');
INSERT INTO `user` VALUES ('31', '1', '28');
INSERT INTO `user` VALUES ('32', '2', '29');
INSERT INTO `user` VALUES ('33', '1', '30');
INSERT INTO `user` VALUES ('34', '1', '31');
INSERT INTO `user` VALUES ('35', '1', '32');
INSERT INTO `user` VALUES ('36', '1', '33');
INSERT INTO `user` VALUES ('37', '1', '34');
INSERT INTO `user` VALUES ('38', '1', '35');
INSERT INTO `user` VALUES ('39', '123', '36');
INSERT INTO `user` VALUES ('40', '1111111111111', '37');
INSERT INTO `user` VALUES ('41', '123', '38');
INSERT INTO `user` VALUES ('42', '123', '39');
INSERT INTO `user` VALUES ('43', '123', '40');
INSERT INTO `user` VALUES ('44', '123', '41');
INSERT INTO `user` VALUES ('45', '123', '42');
INSERT INTO `user` VALUES ('46', '123', '43');
INSERT INTO `user` VALUES ('47', '123', '44');
INSERT INTO `user` VALUES ('48', '123', '56');
INSERT INTO `user` VALUES ('49', '123', '57');
INSERT INTO `user` VALUES ('50', '123', '58');
INSERT INTO `user` VALUES ('51', '123', '59');
INSERT INTO `user` VALUES ('52', '123', '60');

-- ----------------------------
-- Table structure for userdata
-- ----------------------------
DROP TABLE IF EXISTS `userdata`;
CREATE TABLE `userdata` (
  `id` int(10) NOT NULL AUTO_INCREMENT,
  `nickname` varchar(15) NOT NULL,
  `sex` char(4) DEFAULT NULL,
  `age` int(3) DEFAULT '0',
  `name` varchar(30) DEFAULT NULL,
  `StarId` int(2) DEFAULT NULL,
  `BloodTypeId` int(2) DEFAULT NULL,
  `faceid` int(6) DEFAULT '1',
  `FriendshipPolicyId` int(1) DEFAULT '1',
  PRIMARY KEY (`id`),
  KEY `data_start` (`StarId`),
  KEY `data_bloodType` (`BloodTypeId`),
  CONSTRAINT `data_bloodType` FOREIGN KEY (`BloodTypeId`) REFERENCES `bloodtype` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  CONSTRAINT `data_start` FOREIGN KEY (`StarId`) REFERENCES `start` (`id`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of userdata
-- ----------------------------
INSERT INTO `userdata` VALUES ('1', 'nick', '女', '18', null, null, null, '3', '1');
INSERT INTO `userdata` VALUES ('8', '凝', 'm\r\n', '18', '手势\r\n', null, null, '1', '1');
INSERT INTO `userdata` VALUES ('9', '1`', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('10', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('11', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('12', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('13', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('14', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('15', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('16', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('17', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('18', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('19', 'sorrow凝', '女', '19', '', '3', '2', '11', '1');
INSERT INTO `userdata` VALUES ('20', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('21', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('24', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('25', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('26', '1', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('27', '1', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('28', '1', '女', '20', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('29', '2', '女', '2', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('30', '1', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('31', '1', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('32', '1', '女', '18', '小天使', null, null, '1', '1');
INSERT INTO `userdata` VALUES ('33', '1', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('34', '1', null, null, null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('35', '213', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('36', '西瓜', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('37', '香蕉', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('38', '数据库', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('39', 'ss', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('40', 'qq', '女', '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('41', 'a001', '女', '18', '刘宏伟', '6', '1', '83', '1');
INSERT INTO `userdata` VALUES ('42', 'a002', '女', '22', '姜定胜', '6', '3', '3', '1');
INSERT INTO `userdata` VALUES ('43', 'a003', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('44', 'a001', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('45', 'qwe', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('46', 'qwe', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('47', 'so', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('48', 'ss', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('49', 'ss', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('50', 'dd', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('51', 'asdasd', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('52', '123', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('53', '123', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('54', '12', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('55', '123', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('56', '123', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('57', '123', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('58', '123', null, '0', null, null, null, '1', '1');
INSERT INTO `userdata` VALUES ('59', '123', '女', '0', '', '1', '2', '3', '1');
INSERT INTO `userdata` VALUES ('60', 'so123', '女', '0', '', '2', '1', '1', '1');

-- ----------------------------
-- View structure for qq_piracy_userdata
-- ----------------------------
DROP VIEW IF EXISTS `qq_piracy_userdata`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `qq_piracy_userdata` AS select `userdata`.`id` AS `id`,`userdata`.`nickname` AS `nickname`,`userdata`.`sex` AS `sex`,`userdata`.`age` AS `age`,`userdata`.`name` AS `name`,`userdata`.`StarId` AS `StarId`,`userdata`.`BloodTypeId` AS `BloodTypeId`,`userdata`.`faceid` AS `faceid`,`userdata`.`FriendshipPolicyId` AS `FriendshipPolicyId`,`bloodtype`.`BloodType` AS `BloodType`,`start`.`start` AS `start` from ((`userdata` join `bloodtype` on((`userdata`.`BloodTypeId` = `bloodtype`.`id`))) join `start` on((`userdata`.`StarId` = `start`.`id`))) ;

-- ----------------------------
-- View structure for qq_piracy_user_data
-- ----------------------------
DROP VIEW IF EXISTS `qq_piracy_user_data`;
CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `qq_piracy_user_data` AS select `user`.`password` AS `password`,`user`.`dataid` AS `dataid`,`userdata`.`nickname` AS `nickname`,`userdata`.`sex` AS `sex`,`userdata`.`age` AS `age`,`userdata`.`name` AS `name`,`userdata`.`StarId` AS `StarId`,`userdata`.`BloodTypeId` AS `BloodTypeId`,`userdata`.`faceid` AS `faceid`,`userdata`.`FriendshipPolicyId` AS `FriendshipPolicyId`,`user`.`id` AS `id` from (`user` join `userdata` on((`user`.`dataid` = `userdata`.`id`))) ;
