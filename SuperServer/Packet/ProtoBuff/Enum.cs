// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Enum.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Protobuf.Enum {

  /// <summary>Holder for reflection information generated from Enum.proto</summary>
  public static partial class EnumReflection {

    #region Descriptor
    /// <summary>File descriptor for Enum.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EnumReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpFbnVtLnByb3RvEghQcm90b2NvbCqBAQoORUhlcm9DbGFzc1R5cGUSGQoV",
            "RUhFUk9fQ0xBU1NfVFlQRV9OT05FEAASHAoYRUhFUk9fQ0xBU1NfVFlQRV9X",
            "QVJSSU9SEAESGwoXRUhFUk9fQ0xBU1NfVFlQRV9BUkNIRVIQAhIZChVFSEVS",
            "T19DTEFTU19UWVBFX01BR0UQAyqdAQoRRUNyZWF0ZUhlcm9SZXN1bHQSHAoY",
            "RUNSRUFURV9IRVJPX1JFU1VMVF9OT05FEAASHwobRUNSRUFURV9IRVJPX1JF",
            "U1VMVF9TVUNDRVNTEAESJAogRUNSRUFURV9IRVJPX1JFU1VMVF9GQUlMX09W",
            "RVJMQVAQAhIjCh9FQ1JFQVRFX0hFUk9fUkVTVUxUX0ZBSUxfTUlOTUFYEAMq",
            "hwEKC0VPYmplY3RUeXBlEhUKEUVPQkpFQ1RfVFlQRV9OT05FEAASFQoRRU9C",
            "SkVDVF9UWVBFX0hFUk8QARIYChRFT0JKRUNUX1RZUEVfTU9OU1RFUhACEhoK",
            "FkVPQkpFQ1RfVFlQRV9EUk9QX0lURU0QAxIUChBFT0JKRUNUX1RZUEVfTlBD",
            "EAQqeAoORUNyZWF0dXJlU3RhdGUSGAoURUNSRUFUVVJFX1NUQVRFX0lETEUQ",
            "ABIYChRFQ1JFQVRVUkVfU1RBVEVfTU9WRRABEhkKFUVDUkVBVFVSRV9TVEFU",
            "RV9TS0lMTBACEhcKE0VDUkVBVFVSRV9TVEFURV9ESUUQAyrPAQoIRU1vdmVE",
            "aXISEgoORU1PVkVfRElSX05PTkUQABIQCgxFTU9WRV9ESVJfVVAQARIWChJF",
            "TU9WRV9ESVJfVVBfUklHSFQQAhITCg9FTU9WRV9ESVJfUklHSFQQAxIYChRF",
            "TU9WRV9ESVJfUklHSFRfRE9XThAEEhIKDkVNT1ZFX0RJUl9ET1dOEAUSFwoT",
            "RU1PVkVfRElSX0RPV05fTEVGVBAGEhIKDkVNT1ZFX0RJUl9MRUZUEAcSFQoR",
            "RU1PVkVfRElSX0xFRlRfVVAQCCqjAQoQRVNraWxsRmFpbFJlYXNvbhIbChdF",
            "U0tJTExfRkFJTF9SRUFTT05fTk9ORRAAEhsKF0VTS0lMTF9GQUlMX1JFQVNP",
            "Tl9TVFVOEAESGwoXRVNLSUxMX0ZBSUxfUkVBU09OX0RJU1QQAhIbChdFU0tJ",
            "TExfRkFJTF9SRUFTT05fQ09TVBADEhsKF0VTS0lMTF9GQUlMX1JFQVNPTl9D",
            "T09MEAQqnQIKCUVTdGF0VHlwZRITCg9FU1RBVF9UWVBFX05PTkUQABIVChFF",
            "U1RBVF9UWVBFX01BWF9IUBABEhUKEUVTVEFUX1RZUEVfTUFYX01QEAISEQoN",
            "RVNUQVRfVFlQRV9IUBADEhEKDUVTVEFUX1RZUEVfTVAQBBISCg5FU1RBVF9U",
            "WVBFX0FUSxAFEhYKEkVTVEFUX1RZUEVfREVGRU5DRRAGEhkKFUVTVEFUX1RZ",
            "UEVfTU9WRV9TUEVFRBAHEhgKFEVTVEFUX1RZUEVfQVRLX1NQRUVEEAgSJwoj",
            "RVNUQVRfVFlQRV9BRERfQVRLX1NQRUVEX01VTFRJUExJRVIQCRIdChlFU1RB",
            "VF9UWVBFX0JBU0VfQVRLX1NQRUVEEAoqNgoJRU1vdmVUeXBlEhMKD0VNT1ZF",
            "X1RZUEVfTk9ORRAAEhQKEEVNT1ZFX1RZUEVfQ0hBU0UQASpcCg1FTW9uc3Rl",
            "ckdyYWRlEhcKE0VNT05TVEVSX0dSQURFX05PTkUQABIZChVFTU9OU1RFUl9H",
            "UkFERV9OT1JNQUwQARIXChNFTU9OU1RFUl9HUkFERV9SQVJFEAIqRAoNRVRl",
            "bGVwb3J0VHlwZRIXChNFVEVMRVBPUlRfVFlQRV9OT05FEAASGgoWRVRFTEVQ",
            "T1JUX1RZUEVfUkVTUEFXThABKmIKCUVJdGVtVHlwZRITCg9FSVRFTV9UWVBF",
            "X05PTkUQABIUChBFSVRFTV9UWVBFX0VRVUlQEAESFgoSRUlURU1fVFlQRV9D",
            "T05TVU1FEAISEgoORUlURU1fVFlQRV9FVEMQAyqQAgoORUVxdWlwSXRlbVR5",
            "cGUSGQoVRUVRVUlQX0lURU1fVFlQRV9OT05FEAASGwoXRUVRVUlQX0lURU1f",
            "VFlQRV9XRUFQT04QARIbChdFRVFVSVBfSVRFTV9UWVBFX1NISUVMRBACEhsK",
            "F0VFUVVJUF9JVEVNX1RZUEVfSEVMTUVUEAMSGgoWRUVRVUlQX0lURU1fVFlQ",
            "RV9BUk1PUhAEEhoKFkVFUVVJUF9JVEVNX1RZUEVfQk9PVFMQBRIbChdFRVFV",
            "SVBfSVRFTV9UWVBFX0dMT1ZFUxAGEhwKGEVFUVVJUF9JVEVNX1RZUEVfUEVO",
            "REFOVBAHEhkKFUVFUVVJUF9JVEVNX1RZUEVfUklORxAIKrsCCglFU2xvdFR5",
            "cGUSEwoPRVNMT1RfVFlQRV9OT05FEAASFQoRRVNMT1RfVFlQRV9XRUFQT04Q",
            "ARIVChFFU0xPVF9UWVBFX1NISUVMRBACEhUKEUVTTE9UX1RZUEVfSEVMTUVU",
            "EAMSFAoQRVNMT1RfVFlQRV9BUk1PUhAEEhQKEEVTTE9UX1RZUEVfQk9PVFMQ",
            "BRIVChFFU0xPVF9UWVBFX0dMT1ZFUxAGEhYKEkVTTE9UX1RZUEVfUEVOREFO",
            "VBAHEhkKFUVTTE9UX1RZUEVfRklSU1RfUklORxAIEhoKFkVTTE9UX1RZUEVf",
            "U0VDT05EX1JJTkcQCRIUChBFU0xPVF9UWVBFX0VRVUlQEGQSFwoSRVNMT1Rf",
            "VFlQRV9DT05TVU1FEMgBEhMKDkVTTE9UX1RZUEVfRVRDEKwCKnEKEUVQaWNr",
            "dXBGYWlsUmVhc29uEhwKGEVQSUNLVVBfRkFJTF9SRUFTT05fTk9ORRAAEiAK",
            "HEVQSUNLVVBfRkFJTF9SRUFTT05fTk9UX01JTkUQARIcChhFUElDS1VQX0ZB",
            "SUxfUkVBU09OX0ZVTEwQAio+CgxFQWRkSXRlbVR5cGUSFgoSRUFERF9JVEVN",
            "X1RZUEVfTkVXEAASFgoSRUFERF9JVEVNX1RZUEVfT0xEEAEqPwoLRUl0ZW1T",
            "dGF0dXMSFQoRRUlURU1fU1RBVFVTX05PTkUQABIZChVFSVRFTV9TVEFUVVNf",
            "RVFVSVBQRUQQASpeCg9FQ29uc3VtYWJsZVR5cGUSGQoVRUNPTlNVTUFCTEVf",
            "VFlQRV9OT05FEAASFwoTRUNPTlNVTUFCTEVfVFlQRV9IUBABEhcKE0VDT05T",
            "VU1BQkxFX1RZUEVfTVAQAipuCgtFRWZmZWN0VHlwZRIVChFFRUZGRUNUX1RZ",
            "UEVfTk9ORRAAEhcKE0VFRkZFQ1RfVFlQRV9EQU1BR0UQARIVChFFRUZGRUNU",
            "X1RZUEVfSEVBTBACEhgKFEVFRkZFQ1RfVFlQRV9BRERTVEFUEAMqfwoTRUVm",
            "ZmVjdER1cmF0aW9uVHlwZRIeChpFRUZGRUNUX0RVUkFUSU9OX1RZUEVfTk9O",
            "RRAAEiMKH0VFRkZFQ1RfRFVSQVRJT05fVFlQRV9URU1QT1JBUlkQARIjCh9F",
            "RUZGRUNUX0RVUkFUSU9OX1RZUEVfUEVSTUFORU5UEAIqUwoKRVNraWxsVHlw",
            "ZRIUChBFU0tJTExfVFlQRV9OT05FEAASFgoSRVNLSUxMX1RZUEVfQUNUSVZF",
            "EAESFwoTRVNLSUxMX1RZUEVfUEFTU0lWRRACKl4KFEVTa2lsbFByb2plY3Rp",
            "bGVUeXBlEh8KG0VTS0lMTF9QUk9KRUNUSUxFX1RZUEVfTk9ORRAAEiUKIUVT",
            "S0lMTF9QUk9KRUNUSUxFX1RZUEVfUFJPSkVDVElMRRABKkgKCEVOUENUeXBl",
            "EhIKDkVOUENfVFlQRV9OT05FEAASEwoPRU5QQ19UWVBFX1NUT1JFEAESEwoP",
            "RU5QQ19UWVBFX1FVRVNUEAIqlgEKCUVGb250VHlwZRITCg9FRk9OVF9UWVBF",
            "X05PTkUQABITCg9FRk9OVF9UWVBFX0hFQUwQARIZChVFRk9OVF9UWVBFX05P",
            "Uk1BTF9ISVQQAhIbChdFRk9OVF9UWVBFX0NSSVRJQ0FMX0hJVBADEhMKD0VG",
            "T05UX1RZUEVfR09MRBAEEhIKDkVGT05UX1RZUEVfRVhQEAUq9gIKDkVTa2ls",
            "bFNsb3RUeXBlEhkKFUVTS0lMTF9TTE9UX1RZUEVfTk9ORRAAEhsKF0VTS0lM",
            "TF9TTE9UX1RZUEVfTk9STUFMEAESGQoVRVNLSUxMX1NMT1RfVFlQRV9EQVNI",
            "EAISHAoYRVNLSUxMX1NMT1RfVFlQRV9BQ1RJVkUxEAMSHAoYRVNLSUxMX1NM",
            "T1RfVFlQRV9BQ1RJVkUyEAQSHAoYRVNLSUxMX1NMT1RfVFlQRV9BQ1RJVkUz",
            "EAUSHAoYRVNLSUxMX1NMT1RfVFlQRV9BQ1RJVkU0EAYSHQoZRVNLSUxMX1NM",
            "T1RfVFlQRV9QQVNTSVZFMRAHEh0KGUVTS0lMTF9TTE9UX1RZUEVfUEFTU0lW",
            "RTIQCBIdChlFU0tJTExfU0xPVF9UWVBFX1BBU1NJVkUzEAkSHQoZRVNLSUxM",
            "X1NMT1RfVFlQRV9QQVNTSVZFNBAKEh0KGUVTS0lMTF9TTE9UX1RZUEVfUEFT",
            "U0lWRTUQCypjCg5FU2tpbGxBcmVhVHlwZRIZChVFU0tJTExfQVJFQV9UWVBF",
            "X05PTkUQABIbChdFU0tJTExfQVJFQV9UWVBFX1NJTkdMRRABEhkKFUVTS0lM",
            "TF9BUkVBX1RZUEVfQVJFQRACKoEBChVFU2tpbGxVc2FnZVRhcmdldFR5cGUS",
            "IQodRVNLSUxMX1VTQUdFX1RBUkdFVF9UWVBFX1NFTEYQABIhCh1FU0tJTExf",
            "VVNBR0VfVEFSR0VUX1RZUEVfQUxMWRABEiIKHkVTS0lMTF9VU0FHRV9UQVJH",
            "RVRfVFlQRV9FTkVNWRACQheqAhRHb29nbGUuUHJvdG9idWYuRW51bWIGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Google.Protobuf.Enum.EHeroClassType), typeof(global::Google.Protobuf.Enum.ECreateHeroResult), typeof(global::Google.Protobuf.Enum.EObjectType), typeof(global::Google.Protobuf.Enum.ECreatureState), typeof(global::Google.Protobuf.Enum.EMoveDir), typeof(global::Google.Protobuf.Enum.ESkillFailReason), typeof(global::Google.Protobuf.Enum.EStatType), typeof(global::Google.Protobuf.Enum.EMoveType), typeof(global::Google.Protobuf.Enum.EMonsterGrade), typeof(global::Google.Protobuf.Enum.ETeleportType), typeof(global::Google.Protobuf.Enum.EItemType), typeof(global::Google.Protobuf.Enum.EEquipItemType), typeof(global::Google.Protobuf.Enum.ESlotType), typeof(global::Google.Protobuf.Enum.EPickupFailReason), typeof(global::Google.Protobuf.Enum.EAddItemType), typeof(global::Google.Protobuf.Enum.EItemStatus), typeof(global::Google.Protobuf.Enum.EConsumableType), typeof(global::Google.Protobuf.Enum.EEffectType), typeof(global::Google.Protobuf.Enum.EEffectDurationType), typeof(global::Google.Protobuf.Enum.ESkillType), typeof(global::Google.Protobuf.Enum.ESkillProjectileType), typeof(global::Google.Protobuf.Enum.ENPCType), typeof(global::Google.Protobuf.Enum.EFontType), typeof(global::Google.Protobuf.Enum.ESkillSlotType), typeof(global::Google.Protobuf.Enum.ESkillAreaType), typeof(global::Google.Protobuf.Enum.ESkillUsageTargetType), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum EHeroClassType {
    [pbr::OriginalName("EHERO_CLASS_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EHERO_CLASS_TYPE_WARRIOR")] Warrior = 1,
    [pbr::OriginalName("EHERO_CLASS_TYPE_ARCHER")] Archer = 2,
    [pbr::OriginalName("EHERO_CLASS_TYPE_MAGE")] Mage = 3,
  }

  public enum ECreateHeroResult {
    [pbr::OriginalName("ECREATE_HERO_RESULT_NONE")] None = 0,
    [pbr::OriginalName("ECREATE_HERO_RESULT_SUCCESS")] Success = 1,
    [pbr::OriginalName("ECREATE_HERO_RESULT_FAIL_OVERLAP")] FailOverlap = 2,
    [pbr::OriginalName("ECREATE_HERO_RESULT_FAIL_MINMAX")] FailMinmax = 3,
  }

  public enum EObjectType {
    [pbr::OriginalName("EOBJECT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EOBJECT_TYPE_HERO")] Hero = 1,
    [pbr::OriginalName("EOBJECT_TYPE_MONSTER")] Monster = 2,
    [pbr::OriginalName("EOBJECT_TYPE_DROP_ITEM")] DropItem = 3,
    [pbr::OriginalName("EOBJECT_TYPE_NPC")] Npc = 4,
  }

  public enum ECreatureState {
    [pbr::OriginalName("ECREATURE_STATE_IDLE")] Idle = 0,
    [pbr::OriginalName("ECREATURE_STATE_MOVE")] Move = 1,
    [pbr::OriginalName("ECREATURE_STATE_SKILL")] Skill = 2,
    [pbr::OriginalName("ECREATURE_STATE_DIE")] Die = 3,
  }

  public enum EMoveDir {
    [pbr::OriginalName("EMOVE_DIR_NONE")] None = 0,
    [pbr::OriginalName("EMOVE_DIR_UP")] Up = 1,
    [pbr::OriginalName("EMOVE_DIR_UP_RIGHT")] UpRight = 2,
    [pbr::OriginalName("EMOVE_DIR_RIGHT")] Right = 3,
    [pbr::OriginalName("EMOVE_DIR_RIGHT_DOWN")] RightDown = 4,
    [pbr::OriginalName("EMOVE_DIR_DOWN")] Down = 5,
    [pbr::OriginalName("EMOVE_DIR_DOWN_LEFT")] DownLeft = 6,
    [pbr::OriginalName("EMOVE_DIR_LEFT")] Left = 7,
    [pbr::OriginalName("EMOVE_DIR_LEFT_UP")] LeftUp = 8,
  }

  public enum ESkillFailReason {
    [pbr::OriginalName("ESKILL_FAIL_REASON_NONE")] None = 0,
    [pbr::OriginalName("ESKILL_FAIL_REASON_STUN")] Stun = 1,
    [pbr::OriginalName("ESKILL_FAIL_REASON_DIST")] Dist = 2,
    [pbr::OriginalName("ESKILL_FAIL_REASON_COST")] Cost = 3,
    [pbr::OriginalName("ESKILL_FAIL_REASON_COOL")] Cool = 4,
  }

  public enum EStatType {
    [pbr::OriginalName("ESTAT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ESTAT_TYPE_MAX_HP")] MaxHp = 1,
    [pbr::OriginalName("ESTAT_TYPE_MAX_MP")] MaxMp = 2,
    [pbr::OriginalName("ESTAT_TYPE_HP")] Hp = 3,
    [pbr::OriginalName("ESTAT_TYPE_MP")] Mp = 4,
    [pbr::OriginalName("ESTAT_TYPE_ATK")] Atk = 5,
    [pbr::OriginalName("ESTAT_TYPE_DEFENCE")] Defence = 6,
    [pbr::OriginalName("ESTAT_TYPE_MOVE_SPEED")] MoveSpeed = 7,
    [pbr::OriginalName("ESTAT_TYPE_ATK_SPEED")] AtkSpeed = 8,
    [pbr::OriginalName("ESTAT_TYPE_ADD_ATK_SPEED_MULTIPLIER")] AddAtkSpeedMultiplier = 9,
    [pbr::OriginalName("ESTAT_TYPE_BASE_ATK_SPEED")] BaseAtkSpeed = 10,
  }

  public enum EMoveType {
    [pbr::OriginalName("EMOVE_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EMOVE_TYPE_CHASE")] Chase = 1,
  }

  public enum EMonsterGrade {
    [pbr::OriginalName("EMONSTER_GRADE_NONE")] None = 0,
    [pbr::OriginalName("EMONSTER_GRADE_NORMAL")] Normal = 1,
    [pbr::OriginalName("EMONSTER_GRADE_RARE")] Rare = 2,
  }

  public enum ETeleportType {
    [pbr::OriginalName("ETELEPORT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ETELEPORT_TYPE_RESPAWN")] Respawn = 1,
  }

  public enum EItemType {
    [pbr::OriginalName("EITEM_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EITEM_TYPE_EQUIP")] Equip = 1,
    [pbr::OriginalName("EITEM_TYPE_CONSUME")] Consume = 2,
    [pbr::OriginalName("EITEM_TYPE_ETC")] Etc = 3,
  }

  public enum EEquipItemType {
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_WEAPON")] Weapon = 1,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_SHIELD")] Shield = 2,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_HELMET")] Helmet = 3,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_ARMOR")] Armor = 4,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_BOOTS")] Boots = 5,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_GLOVES")] Gloves = 6,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_PENDANT")] Pendant = 7,
    [pbr::OriginalName("EEQUIP_ITEM_TYPE_RING")] Ring = 8,
  }

  public enum ESlotType {
    [pbr::OriginalName("ESLOT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ESLOT_TYPE_WEAPON")] Weapon = 1,
    [pbr::OriginalName("ESLOT_TYPE_SHIELD")] Shield = 2,
    [pbr::OriginalName("ESLOT_TYPE_HELMET")] Helmet = 3,
    [pbr::OriginalName("ESLOT_TYPE_ARMOR")] Armor = 4,
    [pbr::OriginalName("ESLOT_TYPE_BOOTS")] Boots = 5,
    [pbr::OriginalName("ESLOT_TYPE_GLOVES")] Gloves = 6,
    [pbr::OriginalName("ESLOT_TYPE_PENDANT")] Pendant = 7,
    [pbr::OriginalName("ESLOT_TYPE_FIRST_RING")] FirstRing = 8,
    [pbr::OriginalName("ESLOT_TYPE_SECOND_RING")] SecondRing = 9,
    [pbr::OriginalName("ESLOT_TYPE_EQUIP")] Equip = 100,
    [pbr::OriginalName("ESLOT_TYPE_CONSUME")] Consume = 200,
    [pbr::OriginalName("ESLOT_TYPE_ETC")] Etc = 300,
  }

  public enum EPickupFailReason {
    [pbr::OriginalName("EPICKUP_FAIL_REASON_NONE")] None = 0,
    [pbr::OriginalName("EPICKUP_FAIL_REASON_NOT_MINE")] NotMine = 1,
    [pbr::OriginalName("EPICKUP_FAIL_REASON_FULL")] Full = 2,
  }

  public enum EAddItemType {
    [pbr::OriginalName("EADD_ITEM_TYPE_NEW")] New = 0,
    [pbr::OriginalName("EADD_ITEM_TYPE_OLD")] Old = 1,
  }

  public enum EItemStatus {
    [pbr::OriginalName("EITEM_STATUS_NONE")] None = 0,
    [pbr::OriginalName("EITEM_STATUS_EQUIPPED")] Equipped = 1,
  }

  public enum EConsumableType {
    [pbr::OriginalName("ECONSUMABLE_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ECONSUMABLE_TYPE_HP")] Hp = 1,
    [pbr::OriginalName("ECONSUMABLE_TYPE_MP")] Mp = 2,
  }

  public enum EEffectType {
    [pbr::OriginalName("EEFFECT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EEFFECT_TYPE_DAMAGE")] Damage = 1,
    [pbr::OriginalName("EEFFECT_TYPE_HEAL")] Heal = 2,
    [pbr::OriginalName("EEFFECT_TYPE_ADDSTAT")] Addstat = 3,
  }

  public enum EEffectDurationType {
    [pbr::OriginalName("EEFFECT_DURATION_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EEFFECT_DURATION_TYPE_TEMPORARY")] Temporary = 1,
    [pbr::OriginalName("EEFFECT_DURATION_TYPE_PERMANENT")] Permanent = 2,
  }

  public enum ESkillType {
    [pbr::OriginalName("ESKILL_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ESKILL_TYPE_ACTIVE")] Active = 1,
    [pbr::OriginalName("ESKILL_TYPE_PASSIVE")] Passive = 2,
  }

  public enum ESkillProjectileType {
    [pbr::OriginalName("ESKILL_PROJECTILE_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ESKILL_PROJECTILE_TYPE_PROJECTILE")] Projectile = 1,
  }

  public enum ENPCType {
    [pbr::OriginalName("ENPC_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ENPC_TYPE_STORE")] Store = 1,
    [pbr::OriginalName("ENPC_TYPE_QUEST")] Quest = 2,
  }

  public enum EFontType {
    [pbr::OriginalName("EFONT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EFONT_TYPE_HEAL")] Heal = 1,
    [pbr::OriginalName("EFONT_TYPE_NORMAL_HIT")] NormalHit = 2,
    [pbr::OriginalName("EFONT_TYPE_CRITICAL_HIT")] CriticalHit = 3,
    [pbr::OriginalName("EFONT_TYPE_GOLD")] Gold = 4,
    [pbr::OriginalName("EFONT_TYPE_EXP")] Exp = 5,
  }

  public enum ESkillSlotType {
    [pbr::OriginalName("ESKILL_SLOT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_NORMAL")] Normal = 1,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_DASH")] Dash = 2,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_ACTIVE1")] Active1 = 3,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_ACTIVE2")] Active2 = 4,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_ACTIVE3")] Active3 = 5,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_ACTIVE4")] Active4 = 6,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_PASSIVE1")] Passive1 = 7,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_PASSIVE2")] Passive2 = 8,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_PASSIVE3")] Passive3 = 9,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_PASSIVE4")] Passive4 = 10,
    [pbr::OriginalName("ESKILL_SLOT_TYPE_PASSIVE5")] Passive5 = 11,
  }

  public enum ESkillAreaType {
    [pbr::OriginalName("ESKILL_AREA_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ESKILL_AREA_TYPE_SINGLE")] Single = 1,
    [pbr::OriginalName("ESKILL_AREA_TYPE_AREA")] Area = 2,
  }

  public enum ESkillUsageTargetType {
    [pbr::OriginalName("ESKILL_USAGE_TARGET_TYPE_SELF")] Self = 0,
    [pbr::OriginalName("ESKILL_USAGE_TARGET_TYPE_ALLY")] Ally = 1,
    [pbr::OriginalName("ESKILL_USAGE_TARGET_TYPE_ENEMY")] Enemy = 2,
  }

  #endregion

}

#endregion Designer generated code
