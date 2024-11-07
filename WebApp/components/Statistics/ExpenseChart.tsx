'use client'
import React from 'react'
import { CartesianGrid, Legend, Line, LineChart, ResponsiveContainer, Tooltip, XAxis, YAxis } from 'recharts';
interface ChartDataItem {
   date: string; 
  value: number;
}

interface ChartDataProps {
  chartData: ChartDataItem[];
}


const ExpenseChart = ({chartData}: ChartDataProps) => {
  return (
    <div className='border border-gray-700 border-t-0 text-center rounded-b-md'>
     <ResponsiveContainer width="100%" height={300}>
        <LineChart data={chartData}>
          <XAxis dataKey="key" />
          <YAxis allowDecimals={false} />
          <Tooltip 
          contentStyle={{
            backgroundColor: '#000000',
            border: '1px solid #8884d8',
            borderRadius: '5px',
            padding: '10px',
          }}
          labelStyle={{
            color: '#ffff',
            fontWeight: 'bold',
          }}
          itemStyle={{
            color: '#ffff',
            fontSize: '14px',
          }}/>
          <Line 
            type="monotone" 
            dataKey="value" 
            stroke="#8884d8"
            dot={false}
            activeDot={{
              r: 5, 
              stroke: '#8884d8', 
              strokeWidth: 2, 
              fill: '#ffffff'
            }}  />
        </LineChart>
      </ResponsiveContainer>
    </div>
  )
}

export default ExpenseChart