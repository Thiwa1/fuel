-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema pay_bill
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema pay_bill
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `pay_bill` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
USE `pay_bill` ;

-- -----------------------------------------------------
-- Table `pay_bill`.`employees`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pay_bill`.`employees` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `calling_name` VARCHAR(100) NOT NULL,
  `account_name` VARCHAR(100) NOT NULL,
  `employee_number` VARCHAR(50) NOT NULL,
  `bank` VARCHAR(100) NOT NULL,
  `branch` VARCHAR(100) NOT NULL,
  `nic_no` VARCHAR(50) NOT NULL,
  `account_number` VARCHAR(50) NOT NULL,
  `area` VARCHAR(100) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `employee_number` (`employee_number` ASC) VISIBLE,
  UNIQUE INDEX `nic_no` (`nic_no` ASC) VISIBLE,
  UNIQUE INDEX `account_number` (`account_number` ASC) VISIBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `pay_bill`.`schedules`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pay_bill`.`schedules` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(100) NOT NULL,
  `date` DATE NOT NULL,
  `created_at` TIMESTAMP NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `name` (`name` ASC) VISIBLE)
ENGINE = InnoDB
AUTO_INCREMENT = 3
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `pay_bill`.`payments`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pay_bill`.`payments` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `schedule_id` INT NULL DEFAULT NULL,
  `employee_id` INT NOT NULL,
  `amount` DECIMAL(10,2) NOT NULL,
  `payment_date` DATE NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `employee_id` (`employee_id` ASC) VISIBLE,
  INDEX `schedule_id` (`schedule_id` ASC) VISIBLE,
  CONSTRAINT `payments_ibfk_1`
    FOREIGN KEY (`employee_id`)
    REFERENCES `pay_bill`.`employees` (`id`),
  CONSTRAINT `payments_ibfk_2`
    FOREIGN KEY (`schedule_id`)
    REFERENCES `pay_bill`.`schedules` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


-- -----------------------------------------------------
-- Table `pay_bill`.`paysheets`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pay_bill`.`paysheets` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `schedule_id` INT NULL DEFAULT NULL,
  `date` DATE NOT NULL,
  `emp_code` VARCHAR(50) NULL DEFAULT NULL,
  `name` VARCHAR(100) NULL DEFAULT NULL,
  `designation` VARCHAR(100) NULL DEFAULT NULL,
  `working_place` VARCHAR(100) NULL DEFAULT NULL,
  `project_head` VARCHAR(100) NULL DEFAULT NULL,
  `status` VARCHAR(50) NULL DEFAULT NULL,
  `basic_salary` DECIMAL(10,2) NULL DEFAULT NULL,
  `travelling_allowance` DECIMAL(10,2) NULL DEFAULT NULL,
  `vehicle_allowance` DECIMAL(10,2) NULL DEFAULT NULL,
  `arrears` DECIMAL(10,2) NULL DEFAULT NULL,
  `gross_pay` DECIMAL(10,2) NULL DEFAULT NULL,
  `salary_nopay_days` DECIMAL(10,2) NULL DEFAULT NULL,
  `nopay_budgetory` DECIMAL(10,2) NULL DEFAULT NULL,
  `nopay_other` DECIMAL(10,2) NULL DEFAULT NULL,
  `epf_8` DECIMAL(10,2) NULL DEFAULT NULL,
  `salary_advance` DECIMAL(10,2) NULL DEFAULT NULL,
  `staff_loan` DECIMAL(10,2) NULL DEFAULT NULL,
  `communication_deduction` DECIMAL(10,2) NULL DEFAULT NULL,
  `net_pay` DECIMAL(10,2) NULL DEFAULT NULL,
  `hold` VARCHAR(50) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `schedule_id` (`schedule_id` ASC) VISIBLE,
  CONSTRAINT `paysheets_ibfk_1`
    FOREIGN KEY (`schedule_id`)
    REFERENCES `pay_bill`.`schedules` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
