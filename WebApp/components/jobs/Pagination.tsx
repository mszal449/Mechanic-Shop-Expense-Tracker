'use client'
import React from 'react'

interface PaginationProps {
    pageNumber: number,
    totalPages: number,
    setPageNumber: (newPage: number) => void;
}

const Pagination = ({pageNumber, totalPages, setPageNumber} : PaginationProps) => {
    
    const handlePageChange = (newPage: number) => {
        if (newPage > 0 && newPage <= totalPages) {
            setPageNumber(newPage);
        }
    };

  return (
    <div className="flex justify-center mt-4">
        <button
          className="pagination--button"
          onClick={() => handlePageChange(pageNumber - 1)}
          disabled={pageNumber === 1}
        >
          Previous
        </button>
        <span className="px-3 py-2 mx-1">{pageNumber}</span>
        <button
          className="pagination--button"
          onClick={() => handlePageChange(pageNumber + 1)}
          disabled={pageNumber >= totalPages}
        >
          Next
        </button>
      </div>
  )
}

export default Pagination