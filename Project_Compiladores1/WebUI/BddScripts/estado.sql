delimiter $$

CREATE TABLE `estado` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Descripcion` varchar(45) CHARACTER SET latin1 COLLATE latin1_spanish_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Descripcion_UNIQUE` (`Descripcion`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1$$

