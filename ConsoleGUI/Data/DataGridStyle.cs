using System;

namespace ConsoleGUI.Data
{
	public readonly struct DataGridStyle
	{
		public readonly Character? HeaderHorizontalBorder;
		public readonly Character? HeaderVerticalBorder;
		public readonly Character? HeaderIntersectionBorder;
		public readonly Character? CellHorizontalBorder;
		public readonly Character? CellVerticalBorder;
		public readonly Character? CellIntersectionBorder;

		public DataGridStyle(Character? headerHorizontalBorder, Character? headerVerticalBorder, Character? headerIntersectionBorder, Character? cellHorizontalBorder, Character? cellVerticalBorder, Character? cellIntersectionBorder)
		{
			if (headerVerticalBorder.HasValue ^ cellVerticalBorder.HasValue)
				throw new InvalidOperationException($"The {nameof(HeaderVerticalBorder)} and the {nameof(CellVerticalBorder)} have to either both be set or both be left empty.");
			if ((headerHorizontalBorder.HasValue && headerVerticalBorder.HasValue) ^ headerIntersectionBorder.HasValue)
				throw new InvalidOperationException($"The {nameof(HeaderIntersectionBorder)} needs to be set when and only when the {nameof(HeaderHorizontalBorder)} and the {nameof(HeaderVerticalBorder)} are both set");
			if ((cellHorizontalBorder.HasValue && cellVerticalBorder.HasValue) ^ cellIntersectionBorder.HasValue)
				throw new InvalidOperationException($"The {nameof(CellIntersectionBorder)} needs to be set when and only when the {nameof(CellHorizontalBorder)} and the {nameof(CellVerticalBorder)} are both set");

			HeaderHorizontalBorder = headerHorizontalBorder;
			HeaderVerticalBorder = headerVerticalBorder;
			HeaderIntersectionBorder = headerIntersectionBorder;
			CellHorizontalBorder = cellHorizontalBorder;
			CellVerticalBorder = cellVerticalBorder;
			CellIntersectionBorder = cellIntersectionBorder;
		}

		internal bool HasVertivalBorders => HeaderVerticalBorder.HasValue;

		public static DataGridStyle AllBorders => new DataGridStyle(new Character('═'), new Character('│'), new Character('╪'), new Character('─'), new Character('│'), new Character('┼'));
		public static DataGridStyle NoHorizontalCellBorders => new DataGridStyle(new Character('═'), new Character('│'), new Character('╪'), null, new Character('│'), null);
		public static DataGridStyle OnlyHorizontalHeaderBorder => new DataGridStyle(new Character('═'), null, null, null, null, null);
		public static DataGridStyle NoBorders => new DataGridStyle(null, null, null, null, null, null);
	}
}
