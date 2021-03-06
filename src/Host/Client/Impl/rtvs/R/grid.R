# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See LICENSE in the project root for license information.

grid_header_format <- function(x)
	if (is.na(x)) NULL else format(x)

# Given a 2D data object and indices of columns, returns a vector of row indices indicating the appropriate order
# of rows if the data is sorted by the columns specified. A positive column index indicates ascending order, and
# a positive index indicates descending order. If a column cannot be sorted (e.g. because it contains values of
# different types, or otherwise incomparable), it is ignored.
grid_order <- function(x, ...) {
	col_idxs <- c(...)

	# order() will do the heavy lifting, but we need to prepare the arguments for it.
	# Use xtfrm() to get a vector of integers that will sort in the same order as the original data -
	# this allows us to negate those integers to obtain descending order.
	# If the column is a list (and hence can contain heterogenous values that cannot be meaningfully
	# compared), xtfrm will raise an error; handle that by producing a vector of zeroes, which will
	# make that column a no-op for order().
	args <- list()
	for (col_idx in col_idxs) {
		col <- x[, abs(col_idx)]
		args[[length(args) + 1]] <- tryCatch(
			xtfrm(col) * sign(col_idx),
			error = function(e) replicate(length(col), 0))
	}

	do.call(order, args)
}

grid_data <- function(x, rows, cols, row_selector) {
  # If it's a 1D vector, turn it into a single-column 2D matrix, then process as such.
  d <- dim(x);
  if (is.null(d) || length(d) == 1) {
    vp <- grid_data(matrix(x), rows, cols, row_selector)
    vp$is_1d <- TRUE;
    return(vp);
  }

  if (missing(rows)) {
    rows <- 1:d[[1]];
  }
  if (missing(cols)) {
    cols <- 1:d[[2]];
  }

  if (!missing(row_selector)) {
      x <- x[row_selector(x),, drop = FALSE]
  }
  x <- x[rows, cols, drop = FALSE]

  # Process and format values column by column, then flatten the resulting list of character vectors.
  max_length <- 100 - 3
  data <- c(lapply(1:ncol(x), function(i) {
    lapply(format(x[,i], trim = TRUE, justify = "none"), function(s) {
      if (nchar(s) <= max_length) s else paste0(substr(s, 1, max_length), '...', collapse = '')
    })
  }), recursive = TRUE)

  # Any names in the original data will flow through, but we don't want them.
  names(data) <- NULL;

  rn <- row.names(x);
  cn <- colnames(x);

  # Format row names
  x.rownames <- NULL;
  if (length(rn) > 0) {
    x.rownames <- sapply(rn, grid_header_format, USE.NAMES = FALSE);
  }

  # Format column names
  x.colnames <- NULL;
  if (!is.null(cn) && (length(cn) > 0)) {
    x.colnames <- sapply(cn, grid_header_format, USE.NAMES = FALSE);
  }

  # assign return value
  vp <- list();
  vp$row.start <- rows[1];
  vp$row.count <- length(rows);
  vp$row.names <- as.list(x.rownames);
  vp$col.start <- cols[1];
  vp$col.count <- length(cols);
  vp$col.names <- as.list(x.colnames);
  vp$data <- as.list(data);
  vp;
}
