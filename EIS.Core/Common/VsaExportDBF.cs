using EIS.Core.CustomView;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIS.Core.Common
{
    public class BooleanField : FieldSkeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanField"/> class with data source field name the same as the field name.
        /// </summary>
        /// <param name="name">The name.</param>
        public BooleanField(string name)
            : base(name, DbfConstants.LogicalFieldLength, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceName">The data source field name.</param>
        public BooleanField(string name, string sourceName)
            : base(name, DbfConstants.LogicalFieldLength, 0, sourceName)
        { }

        /// <summary>
        /// Gets the field writer instance.
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="DbfLogicalFieldWriter"/>.
        /// </returns>
        protected override IFieldWriter GetFieldWriterInstance(IDataFileExportFormatFactory exportFormatFactory)
        {
            return exportFormatFactory.CreateBooleanFieldWriter(this);
        }
    }

    /// <summary>
    /// Provides a entry point for exporting tabular data to a data file.
    /// </summary>
    public class DataFileExport
    {
        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>The data source.</value>
        public IDataSource DataSource { get; set; }

        private readonly List<IField> _fields = new List<IField>();
        /// <summary>
        /// Gets the fields.
        /// </summary>
        /// <value>The fields.</value>
        public IList<IField> Fields { get { return _fields; } }

        /// <summary>
        /// Gets the data file format.
        /// </summary>
        /// <value>The data file format.</value>
        public IDataFileExportFormatFactory ExportFormatFactory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFileExport"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="exportFormatFactory">The data file format.</param>
        public DataFileExport(IDataSource dataSource, IDataFileExportFormatFactory exportFormatFactory)
        {
            DataSource = dataSource;
            ExportFormatFactory = exportFormatFactory;
        }

        /// <summary>
        /// Creates a <see cref="DataFileExport" /> that writes the specified data source with the specified format.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="dataFileExportFormatFactory">The data file format.</param>
        /// <returns>A new data file export definition.</returns>
        public static DataFileExport Create(IDataSource dataSource, IDataFileExportFormatFactory dataFileExportFormatFactory)
        {
            Guard.Against<ArgumentNullException>(null == dataSource, "dataSource");
            Guard.Against<ArgumentNullException>(null == dataFileExportFormatFactory, "dataFileExportFormatFactory");

            DataFileExport dataFileExport = new DataFileExport(dataSource, dataFileExportFormatFactory);

            return dataFileExport;
        }

        #region Create Overloads

        /// <summary>
        /// Creates a <see cref="DataFileExport" /> that writes data from the source data with DBF file format.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <returns>A new data file export definition.</returns>
        public static DataFileExport CreateDbf(IDataSource dataSource)
        {
            return Create(dataSource, new DbfDataFileExportFormatFactory());
        }

        /// <summary>
        /// Creates a <see cref="DataFileExport" /> that writes data from the source DataSet with DBF file format.
        /// </summary>
        /// <param name="sourceDataSet">The source data set.</param>
        /// <returns>A new data file export definition.</returns>
        public static DataFileExport CreateDbf(DataSet sourceDataSet)
        {
            return CreateDbf(new DataSetDataSource(sourceDataSet));
        }

        /// <summary>
        /// Creates a <see cref="DataFileExport" /> that writes data from the source item list with DBF file format.
        /// </summary>
        /// <param name="sourceItems">The source items.</param>
        /// <returns>A new data file export definition.</returns>
        public static DataFileExport CreateDbf(IList sourceItems)
        {
            return CreateDbf(new PlainObjectsDataSource(sourceItems));
        }


        #endregion


        /// <summary>
        /// Changes the file format to DBF.
        /// </summary>
        /// <returns>The data file export definition.</returns>
        public DataFileExport AsDbf()
        {
            this.ExportFormatFactory = new DbfDataFileExportFormatFactory();
            return this;
        }

        /// <summary>
        /// Writes data from the data source to the specified stream using the current file format.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <returns>The data file export definition.</returns>
        public DataFileExport Write(Stream stream)
        {
            Guard.Against<ArgumentNullException>(null == stream, "stream");

            using (IDataFileWriter writer = this.ExportFormatFactory.CreateWriter(stream))
            {
                writer.Write(this);
            }
            return this;
        }

        /// <summary>
        /// Writes data from the data source to the specified file using the current file format.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The data file export definition.</returns>
        public DataFileExport Write(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Write(stream);
            }
            return this;
        }
    }


    /// <summary>
    /// Provides fluent interface extensions for <see cref="DataFileExport" />.
    /// </summary>
    public static class DataFileExportExtensions
    {
        /// <summary>
        /// Adds a new <see cref="TextField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddTextField(this DataFileExport dataFileExport, string name, byte totalSize)
        {
            TextField field = new TextField(name, totalSize);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="TextField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name and with the default size of 255.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddTextField(this DataFileExport dataFileExport, string name)
        {
            TextField field = new TextField(name);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="TextField"/> to the <see cref="DataFileExport"/>.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddTextField(this DataFileExport dataFileExport, string name, byte totalSize, string sourceName)
        {
            TextField field = new TextField(name, totalSize, sourceName);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="TextField"/> to the <see cref="DataFileExport"/> with the default size of 255.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddTextField(this DataFileExport dataFileExport, string name, string sourceName)
        {
            TextField field = new TextField(name, sourceName);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="NumericField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddNumericField(this DataFileExport dataFileExport, string name, byte totalSize, byte decimalPlaces)
        {
            NumericField field = new NumericField(name, totalSize, decimalPlaces);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="NumericField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name and with the default total size of 255 and 15 decimal places.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddNumericField(this DataFileExport dataFileExport, string name)
        {
            NumericField field = new NumericField(name);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="NumericField"/> to the <see cref="DataFileExport"/>.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddNumericField(this DataFileExport dataFileExport, string name, byte totalSize, byte decimalPlaces, string sourceName)
        {
            NumericField field = new NumericField(name, totalSize, decimalPlaces, sourceName);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="NumericField"/> to the <see cref="DataFileExport"/> with the default total size of 255 and 15 decimal places.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddNumericField(this DataFileExport dataFileExport, string name, string sourceName)
        {
            NumericField field = new NumericField(name, sourceName);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="DateField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddDateField(this DataFileExport dataFileExport, string name)
        {
            DateField field = new DateField(name);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="DateField"/> to the <see cref="DataFileExport"/>.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        /// <remarks>The date field might contain time information but the will be only written if the writter supports it. 
        /// For example DBF date fields contain only the date without time information.</remarks>
        public static DataFileExport AddDateField(this DataFileExport dataFileExport, string name, string sourceName)
        {
            DateField field = new DateField(name, sourceName);

            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="BooleanField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddBooleanField(this DataFileExport dataFileExport, string name)
        {
            BooleanField field = new BooleanField(name);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="BooleanField"/> to the <see cref="DataFileExport"/>.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The name.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddBooleanField(this DataFileExport dataFileExport, string name, string sourceName)
        {
            BooleanField field = new BooleanField(name, sourceName);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="GenericField"/> to this <see cref="DataFileExport"/> with data source field name the same as the field name.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddGenericField(this DataFileExport dataFileExport, string name)
        {
            GenericField field = new GenericField(name);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }

        /// <summary>
        /// Adds a new <see cref="GenericField"/> to the <see cref="DataFileExport"/>.
        /// </summary>
        /// <param name="dataFileExport">The data file export.</param>
        /// <param name="name">The field name.</param>
        /// <param name="sourceName">The data source field name.</param>
        /// <returns>The <see cref="DataFileExport"/>.</returns>
        public static DataFileExport AddGenericField(this DataFileExport dataFileExport, string name, string sourceName)
        {
            GenericField field = new GenericField(name, sourceName);
            dataFileExport.Fields.Add(field);
            return dataFileExport;
        }
    }


    /// <summary>
    /// Implements a <see cref="IDataSource"/> for reading data from a <see cref="DataSet"/>.
    /// </summary>
    public class DataSetDataSource : IDataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSetDataSource"/> class.
        /// </summary>
        /// <param name="sourceDataSet">The source data set.</param>
        public DataSetDataSource(DataSet sourceDataSet)
        {
            Guard.Against<ArgumentNullException>(sourceDataSet == null, "sourceDataSet");
            Guard.Against<ArgumentException>(sourceDataSet.Tables.Count == 0, "sourceDataSet has 0 Tables");

            _sourceDataSet = sourceDataSet;
            _cachedRows = GetDataTable().Rows;
        }

        private string _memberName;
        /// <summary>
        /// Gets or sets the DataTable name.
        /// </summary>
        /// <value>The DataTable name.</value>
        public string MemberName
        {
            get { return _memberName; }
            set
            {
                _memberName = value;
                _cachedRows = GetDataTable().Rows;
            }
        }

        /// <summary>
        /// Gets the row count in the data source.
        /// </summary>
        /// <value>The row count.</value>
        public int RowCount
        {
            get { return this.GetDataTable().Rows.Count; }
        }

        private readonly DataSet _sourceDataSet;
        /// <summary>
        /// Gets the source data set.
        /// </summary>
        /// <value>The source data set.</value>
        public DataSet SourceDataSet
        {
            get { return _sourceDataSet; }
        }

        /// <summary>
        /// Gets the data value for the field name at the specified row index. The data is taken from the <see cref="DataTable"/> specified in MemberName or in the first one if MemberName is not specified.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="name">The field name.</param>
        /// <returns>
        /// The value for the field name at the specified row index.
        /// </returns>
        public object GetData(int rowIndex, string name)
        {
            DataRow row = _cachedRows[rowIndex];
            return row[name];
        }

        /// <summary>
        /// Gets the <see cref="DataTable"/> from the source <see cref="DataSet"/> specified in MemberName or in the first one if MemberName is not specified..
        /// </summary>
        /// <returns>A <see cref="DataTable"/>.</returns>
        DataTable GetDataTable()
        {
            if (String.IsNullOrEmpty(this.MemberName))
            {
                return this.SourceDataSet.Tables[0];
            }
            else
            {
                DataTable dataTable = this.SourceDataSet.Tables[this.MemberName];
                if (null == dataTable) throw new InvalidOperationException(this.MemberName + " Table does not exists in the SourceDataSet.");
                return dataTable;
            }
        }

        private DataRowCollection _cachedRows;
    }

    /// <summary>
    /// Represents a date field.
    /// </summary>
    public class DateField : FieldSkeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateField"/> class with data source field name the same as the field name.
        /// </summary>
        /// <param name="name">The name.</param>
        public DateField(string name)
            : base(name, 8, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceName">The data source field name.</param>
        public DateField(string name, string sourceName)
            : base(name, 8, 0, sourceName)
        { }

        /// <summary>
        /// Gets the field writer instance. 
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="DbfDateFieldWriter"/>.
        /// </returns>
        protected override IFieldWriter GetFieldWriterInstance(IDataFileExportFormatFactory exportFormatFactory)
        {
            return exportFormatFactory.CreateDateFieldWriter(this);
        }
    }

    class DbfCharacterFieldWriter : DbfFieldWriterSkeleton
    {
        public DbfCharacterFieldWriter(DbfDataFileWriter dbfDataFileWriter, IField field)
            : base(dbfDataFileWriter, field)
        { }

        public override char FieldType
        {
            get { return 'C'; }
        }

        public override string FormatValue(object value)
        {
            string stringValue = Convert.ToString(value);
            string fixedLengthValue = Utils.GetFixedLengthString(stringValue, this.Field.TotalSize,
                                                                 DbfConstants.FieldValuePaddingChar,
                                                                 PaddingPositions.Right);
            return fixedLengthValue;
        }
    }

    class DbfConstants
    {
        public static readonly byte HeaderTerminator = 0x0D;
        public static readonly int HeaderTerminatorSize = sizeof(byte); //eg: sizeof(DbfConstants.HeaderTerminator)

        public static readonly byte RowsEndTerminator = 0x1A;
        public static readonly int RowsEndTerminatorSize = sizeof(byte);//eg: sizeof(DbfConstants.RowsEndTerminator)

        public static readonly char FieldNamePaddingChar = (char)0;
        public static readonly char FieldValuePaddingChar = ' ';

        public static readonly int FieldNameLength = 11;

        public static readonly byte DateFieldLength = 8;
        public static readonly byte LogicalFieldLength = 1;

        public static readonly byte DbfVersionDBase3 = 0x03;

        public static readonly byte FieldMaximumLength = 255;
        public static readonly byte FieldMaximumDecimals = 15;
    }

    /// <summary>
    /// Defines methods for creating writers for DBF data file export format.
    /// </summary>
    public class DbfDataFileExportFormatFactory : IDataFileExportFormatFactory
    {
        internal DbfDataFileWriter DbfDataFileWriter { get; private set; }

        /// <summary>
        /// Creates a DBF data file export writer.
        /// </summary>
        /// <param name="stream">The output stream to which the writer will write to.</param>
        /// <returns>A DBF data file writer.</returns>
        public IDataFileWriter CreateWriter(Stream stream)
        {
            if (null != this.DbfDataFileWriter)
                throw new InvalidOperationException("Data file export format factory instance cannot be reused.");
            return this.DbfDataFileWriter = new DbfDataFileWriter(stream);
        }

        /// <summary>
        /// Creates a <see cref="DbfCharacterFieldWriter"/> field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A <see cref="DbfCharacterFieldWriter"/> field writer</returns>
        public IFieldWriter CreateTextFieldWriter(IField field)
        {
            return new DbfCharacterFieldWriter(this.DbfDataFileWriter, field);
        }

        /// <summary>
        /// Creates a <see cref="DbfNumericFieldWriter"/> field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A <see cref="DbfNumericFieldWriter"/> field writer</returns>
        public IFieldWriter CreateNumericFieldWriter(IField field)
        {
            return new DbfNumericFieldWriter(this.DbfDataFileWriter, field);
        }

        /// <summary>
        /// Creates a <see cref="DbfDateFieldWriter"/> field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A <see cref="DbfDateFieldWriter"/> field writer</returns>
        public IFieldWriter CreateDateFieldWriter(IField field)
        {
            return new DbfDateFieldWriter(this.DbfDataFileWriter, field);
        }

        /// <summary>
        /// Creates a <see cref="DbfLogicalFieldWriter"/> field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A <see cref="DbfLogicalFieldWriter"/> field writer</returns>
        public IFieldWriter CreateBooleanFieldWriter(IField field)
        {
            return new DbfLogicalFieldWriter(this.DbfDataFileWriter, field);
        }

        /// <summary>
        /// Creates a <see cref="DbfCharacterFieldWriter"/> field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A <see cref="DbfCharacterFieldWriter"/> field writer</returns>
        public IFieldWriter CreateGenericFieldWriter(IField field)
        {
            return new DbfCharacterFieldWriter(this.DbfDataFileWriter, field);
        }
    }


    /// <summary>
    /// Writes DBF streams.
    /// </summary>
    /// <example>
    /// The folowing example shows how to write a DBF file usind a DataSet as a data source:
    /// <code>
    /// DataSet source = new DataSet();
    /// DataTable table = source.Tables.Add();
    /// table.Columns.Add("AString", typeof(string));
    /// table.Rows.Add("a");
    /// using (FileStream stream = new FileStream(filePath, FileMode.Create))
    /// {
    ///     DbfWriter.Create(new DataSetDataSource(source))
    ///         .AddTextField("AString", 10)
    ///         .Write(stream);
    /// }
    /// </code>
    /// </example>
    public class DbfDataFileWriter : IDataFileWriter, IDisposable
    {
        /// <summary>
        /// Gets the output stream.
        /// </summary>
        /// <value>The output stream.</value>
        public Stream Stream { get; private set; }

        /// <summary>
        /// Gets the binary writer used to write to the output stream.
        /// </summary>
        /// <value>The binary writer.</value>
        public BinaryWriter BinaryWriter { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbfDataFileWriter"/> class.
        /// </summary>
        /// <param name="stream">The output stream to write to.</param>
        public DbfDataFileWriter(Stream stream)
        {
            this.Stream = stream;
            this.BinaryWriter = new BinaryWriter(stream, Encoding.Default);
        }

        /// <summary>
        /// Writes the data file to the output stream.
        /// </summary>
        /// <param name="dataFileExport">The data file export definition.</param>
        public void Write(DataFileExport dataFileExport)
        {
            WriteFileHeader(dataFileExport);
            WriteRows(dataFileExport);

            this.BinaryWriter.Flush();
        }

        void WriteFileHeader(DataFileExport dataFileExport)
        {
            DbfHeaderStructure headerStructure = new DbfHeaderStructure();
            FillDbfFileHeaderStructure(dataFileExport, ref headerStructure);

            DbfStructureBinaryWriter.Write(this.BinaryWriter, headerStructure);

            WriteFieldsHeader(dataFileExport);

            this.BinaryWriter.Write(DbfConstants.HeaderTerminator);
        }

        void WriteFieldsHeader(DataFileExport dataFileExport)
        {
            int offset = 1;
            for (int i = 0; i < dataFileExport.Fields.Count; i++)
            {
                IField field = dataFileExport.Fields[i];
                field.GetFieldWriter(dataFileExport.ExportFormatFactory).WriteHeader(offset);
                offset += field.TotalSize;
            }
        }

        /// <summary>
        /// Ghi dong
        /// </summary>
        /// <param name="dataFileExport"></param>
        void WriteRows(DataFileExport dataFileExport)
        {
            int rowCount = dataFileExport.DataSource.RowCount;
            if (rowCount > 0)
            {
                for (int rowIndex = 0; rowIndex != rowCount; rowIndex++)
                {
                    WriteRow(dataFileExport, rowIndex);
                }

                // rows end separator is written only if there are rows in the data source
                this.BinaryWriter.Write(DbfConstants.RowsEndTerminator);
            }
        }

        /// <summary>
        /// Đây là phương thức trực tiếp ghi nội dung vào file
        /// Ghi từng bản ghi
        /// </summary>
        /// <param name="dataFileExport"></param>
        /// <param name="rowIndex"></param>
        void WriteRow(DataFileExport dataFileExport, int rowIndex)
        {
            // deleted marker
            this.BinaryWriter.Write(DbfDeletedFlags.Valid);

            // values
            for (int fieldIndex = 0; fieldIndex != dataFileExport.Fields.Count; fieldIndex++)
            {
                IField field = dataFileExport.Fields[fieldIndex];
                IFieldWriter fieldWriter = field.GetFieldWriter(dataFileExport.ExportFormatFactory);

                object value = dataFileExport.DataSource.GetData(rowIndex, field.SourceName);





                // Ghi cả đối tượng
                fieldWriter.WriteValue(value);
            }
        }

        static void FillDbfFileHeaderStructure(DataFileExport dataFileExport, ref DbfHeaderStructure hdr)
        {
            hdr.VersionNumber = DbfConstants.DbfVersionDBase3;
            DateTime lastModifiedDate = DateTime.Now;
            hdr.LastUpdateYear = (byte)(lastModifiedDate.Year % 100);
            hdr.LastUpdateMonth = (byte)lastModifiedDate.Month;
            hdr.LastUpdateDay = (byte)lastModifiedDate.Day;
            hdr.EncryptionFlag = 0;
            hdr.HeaderSize = GetHeaderSize(dataFileExport);
            hdr.LanguageDriverId = DbfLanguageDrivers.WindowsAnsi;
            hdr.MdxFlag = 0;
            hdr.NumberOfRecords = dataFileExport.DataSource.RowCount;
            hdr.RecordSize = GetTotalRecordSize(dataFileExport);
            hdr.Reserved1 = 0;
            hdr.IncompleteTransaction = 0;
            hdr.FreeRecordThreadReserved = 0;
            hdr.ReservedMultiUser1 = 0;
            hdr.ReservedMultiUser2 = 0;
            hdr.Reserved2 = 0;
        }

        static short GetTotalRecordSize(DataFileExport dataFileExport)
        {
            int totalSize = dataFileExport.Fields.Sum(f => (int)f.TotalSize);
            return (short)(1 + totalSize);
        }

        static short GetHeaderSize(DataFileExport dataFileExport)
        {
            return (short)(DbfHeaderStructure.StructureSize
                           + DbfConstants.HeaderTerminatorSize
                           + DbfFieldHeaderStructure.StructureSize * dataFileExport.Fields.Count);
        }

        void IDisposable.Dispose()
        {
            this.BinaryWriter.BaseStream.Dispose();
        }
    }

    class DbfDateFieldWriter : DbfFieldWriterSkeleton
    {
        public DbfDateFieldWriter(DbfDataFileWriter dbfDataFileWriter, IField field)
            : base(dbfDataFileWriter, field)
        { }

        public override char FieldType
        {
            get { return 'D'; }
        }

        public const string DataFormatPattern = "yyyyMMdd";

        /// <summary>
        /// Sửa định dạng ngày tháng ở đây
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string FormatValue(object value)
        {
            DateTime dateValue = Convert.ToDateTime(value);
            string formatedValue = dateValue.ToString(DataFormatPattern);

            return formatedValue;
        }
    }

    class DbfDeletedFlags
    {
        public static readonly byte Deleted = Encoding.ASCII.GetBytes("*")[0];
        public static readonly byte Valid = Encoding.ASCII.GetBytes(" ")[0];

        public static readonly int StructureSize = sizeof(byte);
    }

    struct DbfFieldHeaderStructure
    {
        public string FieldName;
        public char FieldType;
        public byte TotalSize;
        public int OffsetInRecord;
        public byte DecimalPlaces;
        public short Reserved1;
        public byte WorkAreaId;
        public short MultiUser;
        public byte SetField;
        public Int32 Reserved21;
        public Int16 Reserver22;
        public byte Reserved23;
        public byte IncludeInMdx;

        public static readonly int StructureSize = 32; // eg: sizeof(DbfFieldHeaderStructure)
    }

    /// <summary>
    /// Cài đặt của IFieldWriter
    /// </summary>
    abstract class DbfFieldWriterSkeleton : IFieldWriter
    {
        public IField Field { get; private set; }

        public abstract char FieldType { get; }

        public DbfDataFileWriter DbfDataFileWriter { get; private set; }

        protected DbfFieldWriterSkeleton(DbfDataFileWriter dbfDataFileWriter, IField field)
        {
            this.DbfDataFileWriter = dbfDataFileWriter;
            this.Field = field;
        }

        public abstract string FormatValue(object value);

        public virtual void WriteValue(object value)
        {
            if (IsNullValue(value))
            {
                WriteNull();
            }
            else
            {
                string formattedValue = this.FormatValue(value);
                //  byte[] bufferToWrite = Encoding.ASCII.GetBytes(formattedValue);

                byte[] bufferToWrite = Encoding.Default.GetBytes(formattedValue);
                this.DbfDataFileWriter.BinaryWriter.Write(bufferToWrite);
            }
        }

        public void WriteHeader(int offsetInRecord)
        {
            DbfFieldHeaderStructure fieldHeaderStructure =
                new DbfFieldHeaderStructure
                {
                    FieldName = this.Field.Name,
                    FieldType = this.FieldType,
                    TotalSize = this.Field.TotalSize,
                    OffsetInRecord = offsetInRecord,
                    DecimalPlaces = this.Field.DecimalPlaces,
                    Reserved1 = 0,
                    WorkAreaId = 0,
                    MultiUser = 0,
                    SetField = 0,
                    Reserved21 = 0,
                    Reserver22 = 0,
                    Reserved23 = 0,
                    IncludeInMdx = 0
                };

            DbfStructureBinaryWriter.Write(this.DbfDataFileWriter.BinaryWriter, fieldHeaderStructure);
        }

        protected bool IsNullValue(object value)
        {
            if (null == value) return true;
            if (DBNull.Value == value) return true;
            return false;
        }

        protected void WriteNull()
        {
            // null values are filled with spaces in DBF
            string nullValueString = Utils.GetFixedLengthString(String.Empty, this.Field.TotalSize, ' ', PaddingPositions.Right);
            this.DbfDataFileWriter.BinaryWriter.Write(Encoding.ASCII.GetBytes(nullValueString));
        }
    }


    struct DbfHeaderStructure
    {
        public byte VersionNumber;
        public byte LastUpdateYear;
        public byte LastUpdateMonth;
        public byte LastUpdateDay;
        public int NumberOfRecords;
        public short HeaderSize;
        public short RecordSize;
        public short Reserved1;
        public byte IncompleteTransaction;
        public byte EncryptionFlag;
        public int FreeRecordThreadReserved;
        public int ReservedMultiUser1;
        public int ReservedMultiUser2;
        public byte MdxFlag;
        public byte LanguageDriverId;
        public short Reserved2;

        public static readonly int StructureSize = 32; // eg: sizeof(DbfHeaderStructure)
    }


    class DbfLanguageDrivers
    {
        public static readonly byte DosUsa = 0x01;
        public static readonly byte DosMultilingual = 0x02;
        public static readonly byte WindowsAnsi = 0x03;
        public static readonly byte StandardMacintosh = 0x04;
        public static readonly byte EeMsDos = 0x64;
        public static readonly byte NordicMsDos = 0x65;
        public static readonly byte RussianMsDos = 0x66;
        public static readonly byte IcelandicMsDos = 0x67;
        public static readonly byte KamenickyCzechMsDos = 0x68;
        public static readonly byte MazoviaPolishMsDos = 0x69;
        public static readonly byte GreekMsDos437G = 0x6A;
        public static readonly byte TurkishMsDos = 0x6B;
        public static readonly byte RussianMacintosh = 0x96;
        public static readonly byte EasternEuropeanMacintosh = 0x97;
        public static readonly byte GreekMacintosh = 0x98;
        public static readonly byte WindowsEe = 0xC8;
        public static readonly byte RussianWindows = 0xC9;
        public static readonly byte TurkishWindows = 0xCA;
        public static readonly byte GreekWindows = 0xCB;
    }


    class DbfLogicalFieldWriter : DbfFieldWriterSkeleton
    {
        public DbfLogicalFieldWriter(DbfDataFileWriter dbfDataFileWriter, IField field)
            : base(dbfDataFileWriter, field)
        { }

        public override char FieldType
        {
            get { return 'L'; }
        }

        public override string FormatValue(object value)
        {
            bool booleanValue = Convert.ToBoolean(value);
            string formattedValue = booleanValue ? DbfLogicalValues.True : DbfLogicalValues.False;
            return formattedValue;
        }
    }


    class DbfLogicalValues
    {
        public const string True = "T";
        public const string False = "F";
    }


    class DbfNumericFieldWriter : DbfFieldWriterSkeleton
    {
        public DbfNumericFieldWriter(DbfDataFileWriter dbfDataFileWriter, IField field)
            : base(dbfDataFileWriter, field)
        { }

        public override char FieldType
        {
            get { return 'N'; }
        }

        public override string FormatValue(object value)
        {
            decimal decimalValue = Convert.ToDecimal(value);
            string decimalPlacesPlaceHolders = String.Empty.PadRight(this.Field.DecimalPlaces, '#');
            string formatString = "#0." + decimalPlacesPlaceHolders;
            string stringValue = decimalValue.ToString(formatString);
            string formatedValue = Utils.GetFixedLengthString(stringValue, this.Field.TotalSize,
                                                              DbfConstants.FieldValuePaddingChar, PaddingPositions.Left);
            return formatedValue;
        }
    }


    static class DbfStructureBinaryWriter
    {
        public static void Write(BinaryWriter binaryWriter, DbfFieldHeaderStructure fieldHeaderStructure)
        {
            string fieldName = Utils.GetFixedLengthString(fieldHeaderStructure.FieldName,
                                                          DbfConstants.FieldNameLength,
                                                          DbfConstants.FieldNamePaddingChar,
                                                          PaddingPositions.Right);
            binaryWriter.Write(Encoding.ASCII.GetBytes(fieldName));
            binaryWriter.Write(fieldHeaderStructure.FieldType);
            binaryWriter.Write(fieldHeaderStructure.OffsetInRecord);
            binaryWriter.Write(fieldHeaderStructure.TotalSize);
            binaryWriter.Write(fieldHeaderStructure.DecimalPlaces);
            binaryWriter.Write(fieldHeaderStructure.Reserved1);
            binaryWriter.Write(fieldHeaderStructure.WorkAreaId);
            binaryWriter.Write(fieldHeaderStructure.MultiUser);
            binaryWriter.Write(fieldHeaderStructure.SetField);
            binaryWriter.Write(fieldHeaderStructure.Reserved21);
            binaryWriter.Write(fieldHeaderStructure.Reserver22);
            binaryWriter.Write(fieldHeaderStructure.Reserved23);
            binaryWriter.Write(fieldHeaderStructure.IncludeInMdx);
        }

        public static void Write(BinaryWriter binaryWriter, DbfHeaderStructure headerStructure)
        {
            binaryWriter.Write(headerStructure.VersionNumber);
            binaryWriter.Write(headerStructure.LastUpdateYear);
            binaryWriter.Write(headerStructure.LastUpdateMonth);
            binaryWriter.Write(headerStructure.LastUpdateDay);
            binaryWriter.Write(headerStructure.NumberOfRecords);
            binaryWriter.Write(headerStructure.HeaderSize);
            binaryWriter.Write(headerStructure.RecordSize);
            binaryWriter.Write(headerStructure.Reserved1);
            binaryWriter.Write(headerStructure.IncompleteTransaction);
            binaryWriter.Write(headerStructure.EncryptionFlag);
            binaryWriter.Write(headerStructure.FreeRecordThreadReserved);
            binaryWriter.Write(headerStructure.ReservedMultiUser1);
            binaryWriter.Write(headerStructure.ReservedMultiUser2);
            binaryWriter.Write(headerStructure.MdxFlag);
            binaryWriter.Write(headerStructure.LanguageDriverId);
            binaryWriter.Write(headerStructure.Reserved2);
        }
    }



    /// <summary>
    /// Provides a skeleton implementation of IField interface.
    /// </summary>
    public abstract class FieldSkeleton : IField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSkeleton"/> class with data source field name the same as the field name.
        /// </summary>
        /// <param name="name">The field name and the data source field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="decimalPlaces">The field decimal places for numeric types. For other types it should be 0.</param>
        protected FieldSkeleton(string name, byte totalSize, byte decimalPlaces)
            : this(name, totalSize, decimalPlaces, name)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldSkeleton"/> class.
        /// </summary>
        /// <param name="name">The field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="decimalPlaces">The field decimal places for numeric types. For other types it should be 0.</param>
        /// <param name="sourceName">Name of the source data field name.</param>
        protected FieldSkeleton(string name, byte totalSize, byte decimalPlaces, string sourceName)
        {
            Guard.Against<ArgumentNullException>(name == null, "name");
            Guard.Against<ArgumentNullException>(sourceName == null, "sourceName");
            Guard.Against<ArgumentException>(totalSize <= decimalPlaces, "totalSize shoud be bigger than decimalPlaces");
            Guard.Against<ArgumentException>(decimalPlaces > DbfConstants.FieldMaximumDecimals, "decimalPlaces should be maximum 15");

            this.Name = name;
            this.TotalSize = totalSize;
            this.DecimalPlaces = decimalPlaces;
            this.SourceName = sourceName;
        }

        /// <summary>
        /// Gets or sets the DBF field header name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the source name.
        /// </summary>
        /// <value>The source name.</value>
        public string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the total size of the field values. For numeric type this include the . decimal separator.
        /// </summary>
        /// <value>The total size.</value>
        public byte TotalSize { get; set; }

        /// <summary>
        /// Gets or sets the decimal places for numeric types. For other types the value is 0.
        /// </summary>
        /// <value>The decimal places.</value>
        public byte DecimalPlaces { get; set; }

        /// <summary>
        /// Gets the field writer instance. Should be overriden to return a new instance of the field writer. The instance is cached by the <see cref="FieldSkeleton"/> class.
        /// </summary>
        /// <returns>A new instance of a <see cref="IFieldWriter"/> implementation.</returns>
        protected abstract IFieldWriter GetFieldWriterInstance(IDataFileExportFormatFactory exportFormatFactory);

        /// <summary>
        /// Gets the cached field writer.
        /// </summary>
        /// <returns>A IFieldWriter.</returns>
        public IFieldWriter GetFieldWriter(IDataFileExportFormatFactory exportFormatFactory)
        {
            if (null == _fieldWriter || exportFormatFactory != _cachedFormat)
            {
                _cachedFormat = exportFormatFactory;
                _fieldWriter = GetFieldWriterInstance(exportFormatFactory);
            }
            return _fieldWriter;
        }

        private IFieldWriter _fieldWriter;
        private IDataFileExportFormatFactory _cachedFormat;
    }


    /// <summary>
    /// Represents a generic field.
    /// </summary>
    public class GenericField : FieldSkeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericField"/> class with data source field name the same as the field name.
        /// </summary>
        /// <param name="name">The field name and the data source field name.</param>
        public GenericField(string name)
            : base(name, NumericField.MaximumTotalSize, NumericField.MaximumDecimalPlaces)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceName">The data source field name.</param>
        public GenericField(string name, string sourceName)
            : base(name, NumericField.MaximumTotalSize, NumericField.MaximumDecimalPlaces, sourceName)
        { }

        /// <summary>
        /// Gets the field writer instance. 
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="IFieldWriter"/>.
        /// </returns>
        protected override IFieldWriter GetFieldWriterInstance(IDataFileExportFormatFactory exportFormatFactory)
        {
            return exportFormatFactory.CreateGenericFieldWriter(this);
        }
    }


    /// <summary>
    /// Helper class for guard statements, which allow prettier
    /// code for guard clauses.
    /// </summary>
    public class Guard
    {
        /// <summary>
        /// Will throw exception of type <typeparamref name="TException"/>
        /// with the specified message if the assertion is true
        /// </summary>
        /// <typeparam name="TException">The exceotion type to throw when the assertion is false.</typeparam>
        /// <param name="assertion">if set to <c>true</c> throws the <typeparamref name="TException"/> exception.</param>
        /// <param name="message">The exception message.</param>
        /// <example>
        /// Sample usage:
        /// <code>
        /// <![CDATA[
        /// Guard.Against<ArgumentException>(string.IsNullOrEmpty(name), "Name must have a value");
        /// ]]>
        /// </code>
        /// </example>
        public static void Against<TException>(bool assertion, string message) where TException : Exception
        {
            if (assertion == false)
                return;
            throw (TException)Activator.CreateInstance(typeof(TException), message);
        }
    }


    /// <summary>
    /// Defines methods for creating writers for specific data file export format.
    /// </summary>
    public interface IDataFileExportFormatFactory
    {
        /// <summary>
        /// Creates a data file export writer.
        /// </summary>
        /// <param name="stream">The output stream to which the writer will write to.</param>
        /// <returns>A data file writer.</returns>
        IDataFileWriter CreateWriter(Stream stream);

        /// <summary>
        /// Creates a text field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A text field writer</returns>
        IFieldWriter CreateTextFieldWriter(IField field);

        /// <summary>
        /// Creates a numeric field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A numeric field writer</returns>
        IFieldWriter CreateNumericFieldWriter(IField field);

        /// <summary>
        /// Creates a date field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A date field writer.</returns>
        IFieldWriter CreateDateFieldWriter(IField field);

        /// <summary>
        /// Creates a boolean field writer.
        /// </summary>
        /// <param name="field">The field for which to create the field writer.</param>
        /// <returns>A boolean field writer.</returns>
        IFieldWriter CreateBooleanFieldWriter(IField field);

        /// <summary>
        /// Creates a generic field writer.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns>A generic field writer.</returns>
        IFieldWriter CreateGenericFieldWriter(IField field);
    }


    /// <summary>
    /// Writes tabular data to a data file.
    /// </summary>
    public interface IDataFileWriter : IDisposable
    {
        /// <summary>
        /// Writes the data to the specified stream.
        /// </summary>
        /// <param name="dataFileExport">The data file export definition.</param>
        void Write(DataFileExport dataFileExport);
    }


    /// <summary>
    /// Defines a data source for the DBF writer.
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// Gets the row count in the data source.
        /// </summary>
        /// <value>The row count.</value>
        int RowCount { get; }

        /// <summary>
        /// Gets the data value for the field name at the specified row index.
        /// </summary>
        /// <param name="rowIndex">The row index.</param>
        /// <param name="name">The field name.</param>
        /// <returns>The value for the field name at the specified row index.</returns>
        object GetData(int rowIndex, string name);
    }


    /// <summary>
    /// Defines a DBF field.
    /// </summary>
    public interface IField
    {
        /// <summary>
        /// Gets or sets the DBF field header name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the source field name.
        /// </summary>
        /// <value>The source field name.</value>
        /// <remarks>
        /// The source field name is used to get data from a <see cref="IDataSource"/>.
        /// </remarks>
        string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the total size of the field values. For numeric type this include the . decimal separator.
        /// </summary>
        /// <value>The total size.</value>
        byte TotalSize { get; set; }

        /// <summary>
        /// Gets or sets the decimal places for numeric types. For other types the value is 0.
        /// </summary>
        /// <value>The decimal places.</value>
        byte DecimalPlaces { get; set; }

        /// <summary>
        /// Gets the field writer.
        /// </summary>
        /// <returns>A IFieldWriter.</returns>
        IFieldWriter GetFieldWriter(IDataFileExportFormatFactory exportFormatFactory);
    }



    /// <summary>
    /// 
    /// class nào cài đặt inteface này
    /// Represents a writer that can write field header and values to a <see cref="IStreamWriter"/>.
    /// </summary>
    public interface IFieldWriter
    {
        /// <summary>
        /// Writes the field header.
        /// </summary>
        /// <param name="offsetInRecord">The offset in record.</param>
        void WriteHeader(int offsetInRecord);

        /// <summary>
        /// Writes the value.
        /// </summary>
        /// <param name="value">The value.</param>
        void WriteValue(object value);
    }


    /// <summary>
    /// Represents a numeric field.
    /// </summary>
    public class NumericField : FieldSkeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericField"/> class with data source field name the same as the field name.
        /// </summary>
        /// <param name="name">The field name and the data source field name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="decimalPlaces">The field decimal places.</param>
        public NumericField(string name, byte totalSize, byte decimalPlaces)
            : base(name, totalSize, decimalPlaces)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericField"/> class with data source field name the same as the field name and with the default total size of 255 and 15 decimal places.
        /// </summary>
        /// <param name="name">The field name and the data source field name.</param>
        public NumericField(string name)
            : base(name, MaximumTotalSize, MaximumDecimalPlaces)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="totalSize">The total size.</param>
        /// <param name="decimalPlaces">The decimal places.</param>
        /// <param name="sourceName">The data source field name.</param>
        public NumericField(string name, byte totalSize, byte decimalPlaces, string sourceName)
            : base(name, totalSize, decimalPlaces, sourceName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericField"/> class with the default total size of 255 and 15 decimal places.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceName">The data source field name.</param>
        public NumericField(string name, string sourceName)
            : base(name, MaximumTotalSize, MaximumDecimalPlaces, sourceName)
        { }

        /// <summary>
        /// Gets the maximum total size for numeric fields.
        /// </summary>
        public const byte MaximumTotalSize = 255;

        /// <summary>
        /// Gets the maximum decimal places for numeric fields.
        /// </summary>
        public const byte MaximumDecimalPlaces = 15;

        /// <summary>
        /// Gets the field writer instance. 
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="DbfNumericFieldWriter"/>.
        /// </returns>
        protected override IFieldWriter GetFieldWriterInstance(IDataFileExportFormatFactory exportFormatFactory)
        {
            return exportFormatFactory.CreateNumericFieldWriter(this);
        }
    }


    /// <summary>
    /// Implements a <see cref="IDataSource"/> for reading data from a list of objects. 
    /// </summary>
    /// <remarks>
    /// The objects in the source list should have public properties that are accesed by name to retrieve the values.
    /// </remarks>
    public class PlainObjectsDataSource : IDataSource
    {
        /// <summary>
        /// Gets the data source list of items.
        /// </summary>
        /// <value>A <see cref="IList"/> of objects.</value>
        public IList Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainObjectsDataSource"/> class.
        /// </summary>
        /// <param name="items">The list of items.</param>
        public PlainObjectsDataSource(IList items)
        {
            Guard.Against<ArgumentNullException>(items == null, "items");

            this.Items = items;
            if (items.Count > 0)
            {
                _cachedType = items[0].GetType();
                _cachedProperties = new Dictionary<string, PropertyInfo>();
            }
        }

        /// <summary>
        /// Gets the row count in the data source.
        /// </summary>
        /// <value>The row count.</value>
        public int RowCount
        {
            get { return this.Items.Count; }
        }

        /// <summary>
        /// Gets the data value for the field name at the specified index.
        /// </summary>
        /// <param name="index">The index in list.</param>
        /// <param name="name">The object property name.</param>
        /// <returns>
        /// The value for the field name at the specified row index.
        /// </returns>
        public object GetData(int index, string name)
        {
            object obj = this.Items[index];
            object value = GetCachedProperty(name).GetValue(obj, null);
            return value;
        }

        PropertyInfo GetCachedProperty(string name)
        {
            PropertyInfo property;
            if (!_cachedProperties.TryGetValue(name, out property))
            {
                property = _cachedType.GetProperty(name);
                _cachedProperties.Add(name, property);
            }
            if (null == property)
            {
                throw new InvalidOperationException("Property '" + name + "' not found in '" + _cachedType.FullName + "'.");
            }
            return property;
        }

        private readonly Dictionary<string, PropertyInfo> _cachedProperties;
        private readonly Type _cachedType;
    }


    /// <summary>
    /// Represents a character field.
    /// </summary>
    public class TextField : FieldSkeleton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class with data source field name the same as the field name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="totalSize">The field total size.</param>
        public TextField(string name, byte totalSize)
            : base(name, totalSize, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class with data source field name the same as the field name and with the default size of 255.
        /// </summary>
        /// <param name="name">The name.</param>
        public TextField(string name)
            : base(name, MaximumTotalSize, 0)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="totalSize">The field total size.</param>
        /// <param name="sourceName">The data source field name.</param>
        public TextField(string name, byte totalSize, string sourceName)
            : base(name, totalSize, 0, sourceName)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextField"/> class with the default size of 255.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sourceName">The data source field name.</param>
        public TextField(string name, string sourceName)
            : base(name, MaximumTotalSize, 0, sourceName)
        { }

        /// <summary>
        /// Gets the maximum total size for character fields.
        /// </summary>
        public const byte MaximumTotalSize = 255;

        /// <summary>
        /// Gets the character writer instance. 
        /// </summary>
        /// <returns>
        /// A new instance of a <see cref="DbfCharacterFieldWriter"/>.
        /// </returns>
        protected override IFieldWriter GetFieldWriterInstance(IDataFileExportFormatFactory exportFormatFactory)
        {
            return exportFormatFactory.CreateTextFieldWriter(this);
        }
    }


    static class Utils
    {
        /// <summary>
        /// Gets a string with a fixed length, padding or shrinking if necessary.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="length">The desired length.</param>
        /// <param name="paddingChar">The padding char used if the input string value is smaller than the desired length.</param>
        /// <param name="paddingPosition">The padding position specifies where the string will be padded when the input string value is smaller than the desired length.</param>
        /// <returns>A string value with the specified length.</returns>
        public static string GetFixedLengthString(string value, int length, char paddingChar, PaddingPositions paddingPosition)
        {
            Guard.Against<ArgumentNullException>(value == null, "value");
            Guard.Against<ArgumentException>(length < 0, "length < 0");

            string final;

            if (value.Length < length)
            {
                if (paddingPosition == PaddingPositions.Right)
                {
                    final = value.PadRight(length, paddingChar);
                }
                else if (paddingPosition == PaddingPositions.Left)
                {
                    final = value.PadLeft(length, paddingChar);
                }
                else
                {
                    throw new NotImplementedException(paddingPosition.ToString());
                }
            }
            else if (value.Length > length)
            {
                final = value.Substring(0, length);
            }
            else
            {
                final = value;
            }

            return final;
        }
    }

    enum PaddingPositions
    {
        Left,
        Right
    }


    public class VsaExportDBF
    {
        public static void fn_Export(string path, IQueryable<DK_CosoKCB_Report> records)
        {
            DataFileExport dataFileExport;

            DataSet _sourceDataSet = GetTestDataSet(records);

            dataFileExport = DataFileExport.CreateDbf(_sourceDataSet);

            dataFileExport.AddTextField("STT", 50)
                         .AddTextField("MA", 150)
                          .AddTextField("TEN", 150)
                          .AddTextField("TUYENCMKT", 50)
                          .AddTextField("HANGBV", 50)
                          .AddTextField("LOAIBV", 150)
                          .AddTextField("TENHUYEN", 150)
                          .AddTextField("DONVI", 150)
                          .AddTextField("MADINHDANH", 50)
                          .AddTextField("MACAPTREN", 50)
                           .AddTextField("DIACHI", 250)
                           .AddTextField("TTDUYET", 50)
                          .AddTextField("HL", 20)
                       .AddTextField("TUCHU", 20)
                       .AddTextField("TRANGTHAI", 40)
                       .AddTextField("HANGDV", 150)
                       .AddTextField("HANGTHUOC", 150)
                       .AddTextField("DKKCBBD", 50)
                       .AddTextField("HTTCHUC", 50)
                       .AddTextField("HINHTHUCTT", 50)
                       .AddTextField("NGAYCAPMA", 50)
                       .AddTextField("KCB", 50)
                       .AddTextField("NGAYNHD", 50)
                       .AddTextField("KT7", 20)
                       .AddTextField("KCN", 20)
                       .AddTextField("KNL", 20)
                       .AddTextField("CPDTT43", 50)
                        .AddTextField("SLTBHDACAP", 50)
                        .AddTextField("DVCHUQUAN", 150)
                       .AddTextField("MOTA", 250)
                          .Write(path);
        }

        private static DataSet GetTestDataSet(IQueryable<DK_CosoKCB_Report> records)
        {
            var i = 1;
            DataSet sourceDataSet = new DataSet("TestDataSet");
            DataTable table = sourceDataSet.Tables.Add("TestDataTable");

            // Thêm cột
            table.Columns.Add("STT", typeof(string));
            table.Columns.Add("MA", typeof(string));
            table.Columns.Add("TEN", typeof(string));
            table.Columns.Add("TUYENCMKT", typeof(string));
            table.Columns.Add("HANGBV", typeof(string));
            table.Columns.Add("LOAIBV", typeof(string));
            table.Columns.Add("TENHUYEN", typeof(string));
            table.Columns.Add("DONVI", typeof(string));
            table.Columns.Add("MADINHDANH", typeof(string));
            table.Columns.Add("MACAPTREN", typeof(string));
            table.Columns.Add("DIACHI", typeof(string));
            table.Columns.Add("TTDUYET", typeof(string));
            table.Columns.Add("HL", typeof(string));
            table.Columns.Add("TUCHU", typeof(string));
            table.Columns.Add("TRANGTHAI", typeof(string));
            table.Columns.Add("HANGDV", typeof(string));
            table.Columns.Add("HANGTHUOC", typeof(string));
            table.Columns.Add("DKKCBBD", typeof(string));
            table.Columns.Add("HTTCHUC", typeof(string));
            table.Columns.Add("HINHTHUCTT", typeof(string));
            table.Columns.Add("NGAYCAPMA", typeof(string));
            table.Columns.Add("KCB", typeof(string));
            table.Columns.Add("NGAYNHD", typeof(string));
            table.Columns.Add("KT7", typeof(string));
            table.Columns.Add("KCN", typeof(string));
            table.Columns.Add("KNL", typeof(string));
            table.Columns.Add("CPDTT43", typeof(string));
            table.Columns.Add("SLTBHDACAP", typeof(string));
            table.Columns.Add("DVCHUQUAN", typeof(string));
            table.Columns.Add("MOTA", typeof(string));


            // Lặp từng dòng đổ dữ liệu từ records vào dt
            foreach (var v in records)
            {
                table.Rows.Add(i++, v.MA, v.TEN, v.TUYENCMKT, v.HANGBENHVIEN, v.LOAIBENHVIEN, v.QUANHUYEN_ID, v.DONVI_ID, v.MABHYT, v.MACOSOKCBCHA, v.DIACHI, v.TTPHEDUYET, v.HIEULUC, v.TUCHU, v.TRANGTHAI, v.HANGDICHVU_TD,
                v.HANGTHUOC_TD, v.DKKCBBD, v.KIEUBV, v.HINHTHUCTT, v.NGAYCAPMA, v.KCB, v.NGAYNGUNGHD, v.KHAMT7, v.KHAMCN, v.KHAMNGAYLE, v.CHUA_PD43, v.SL_THE_BH_DA_CAP, v.LOAI_DONVICHUQUAN, v.MIEUTA);
            }

            return sourceDataSet;
        }


        public static void fn_ExportBhyt14(string path, BHYT14View14Portal records)
        {
            var sourceDataSet = GetTestDataSetBhyt14(records);

            var dataFileExport = DataFileExport.CreateDbf(sourceDataSet);

            dataFileExport.AddTextField("STT", 50)
                         .AddTextField("LOAIKCB", 150)
                         .AddTextField("TEN_CSKCB", 250)
                          .AddTextField("MA_CSKCB", 50)
                          .AddTextField("SOLUOT_DT", 250)
                          .AddTextField("SOLUOT_TT", 250)
                          .AddTextField("SONGAY_DT", 250)
                          .AddTextField("CP_TCONG", 250)
                          .AddTextField("CP_XN", 20)
                          .AddTextField("CP_CDHA", 250)
                           .AddTextField("CP_THUOC", 250)
                           .AddTextField("CP_MAU", 250)
                          .AddTextField("CP_PTTT", 250)
                       .AddTextField("CP_VTYT", 250)
                       .AddTextField("CP_DVKT_TL", 250)
                       .AddTextField("CP_THUOCTL", 250)
                       .AddTextField("CP_VTYT_TL", 250)
                       .AddTextField("CP_TKTG", 250)
                       .AddTextField("CP_VCHUYEN", 250)
                       .AddTextField("CP_BNTT", 250)
                       .AddTextField("CP_BHTT", 250)
                       .AddTextField("TUCHOI_TT", 250)
                       .AddTextField("TT_SOLUOT", 250)
                       .AddTextField("TT_BHTT", 250)
                       .AddTextField("TT_BNTT", 250)
                          .Write(path);
        }

        private static DataSet GetTestDataSetBhyt14(BHYT14View14Portal records)
        {
            var i = 1;
            var sourceDataSet = new DataSet("TestDataSet");
            var table = sourceDataSet.Tables.Add("TestDataTable");

            // Thêm cột
            table.Columns.Add("STT", typeof(string));
            table.Columns.Add("LOAIKCB", typeof(string));
            table.Columns.Add("TEN_CSKCB", typeof(string));
            table.Columns.Add("MA_CSKCB", typeof(string));
            table.Columns.Add("SOLUOT_DT", typeof(string));
            table.Columns.Add("SOLUOT_TT", typeof(string));
            table.Columns.Add("SONGAY_DT", typeof(string));
            table.Columns.Add("CP_TCONG", typeof(string));
            table.Columns.Add("CP_XN", typeof(string));
            table.Columns.Add("CP_CDHA", typeof(string));
            table.Columns.Add("CP_THUOC", typeof(string));
            table.Columns.Add("CP_MAU", typeof(string));
            table.Columns.Add("CP_PTTT", typeof(string));
            table.Columns.Add("CP_VTYT", typeof(string));
            table.Columns.Add("CP_DVKT_TL", typeof(string));
            table.Columns.Add("CP_THUOCTL", typeof(string));
            table.Columns.Add("CP_VTYT_TL", typeof(string));
            table.Columns.Add("CP_TKTG", typeof(string));
            table.Columns.Add("CP_VCHUYEN", typeof(string));
            table.Columns.Add("CP_BNTT", typeof(string));
            table.Columns.Add("CP_BHTT", typeof(string));
            table.Columns.Add("TUCHOI_TT", typeof(string));
            table.Columns.Add("TT_SOLUOT", typeof(string));
            table.Columns.Add("TT_BHTT", typeof(string));
            table.Columns.Add("TT_BNTT", typeof(string));


            foreach (var v in records.Datasource)
            {
                table.Rows.Add(i++, v.LOAI_KCB, v.TEN_CSKCB, v.MA_CSKCB,
                    v.LUOT_DUNG_TUYEN,
                    v.LUOT_TRAI_TUYEN, v.SO_NGAY_DTRI, v.T_TONGCHI,
                    v.K_XETNGHIEM, v.K_CDHATDCN, v.K_THUOC, v.K_MAU,
                    v.K_PTTT, v.K_VTYT, v.H_DVKT, v.H_THUOC,
                v.H_VTYT, v.T_KHAM_GIUONG, v.TIENVANCHUYEN, v.T_BNTT,
                v.T_BHTT, v.T_18, v.T_19, v.T_20, v.T_21);
            }

            return sourceDataSet;
        }


    }

}
