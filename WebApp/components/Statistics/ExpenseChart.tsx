import React from 'react'

interface ChartDataItem {
  key: string; 
  value: number;
}

interface ChartDataProps {
  chartData: ChartDataItem[];
}


const ExpenseChart = ({chartData}: ChartDataProps) => {
  console.log(chartData)
  return (
    <div className='border border-gray-700 border-t-0 text-center rounded-b-md'>
    </div>
  )
}

export default ExpenseChart