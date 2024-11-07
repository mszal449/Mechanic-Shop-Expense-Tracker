import React from 'react'

interface JobsSummaryProps {
  totalJobs: number
  pendingJobs: number
  completedJobs: number
  inProgressJobs: number,
  cancelledJobs: number
}

const JobsSummary = async ({
  totalJobs, 
  pendingJobs, 
  completedJobs, 
  inProgressJobs, 
  cancelledJobs}: JobsSummaryProps) => {
  return (
    <div className="text-center p-4 gap-5 grid grid-cols-4 border border-gray-700 rounded-t-md border-b-0">
      <div className='statistics--jobsCountElement'>Total Jobs: {totalJobs}</div>
      <div className='statistics--jobsCountElement'>Pending Jobs: {pendingJobs}</div>
      <div className='statistics--jobsCountElement'>Completed Jobs: {completedJobs}</div>
      <div className='statistics--jobsCountElement'>In Progress Jobs: {inProgressJobs}</div>
      <div className='statistics--jobsCountElement'>Cancelled Jobs: {cancelledJobs}</div>
    </div>
  )
}

export default JobsSummary