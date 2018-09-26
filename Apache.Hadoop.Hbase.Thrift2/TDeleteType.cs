/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */

namespace Apache.Hadoop.Hbase.Thrift2
{
  /// <summary>
  /// Specify type of delete:
  ///  - DELETE_COLUMN means exactly one version will be removed,
  ///  - DELETE_COLUMNS means previous versions will also be removed.
  /// </summary>
  public enum TDeleteType
  {
    DELETE_COLUMN = 0,
    DELETE_COLUMNS = 1,
    DELETE_FAMILY = 2,
    DELETE_FAMILY_VERSION = 3,
  }
}
