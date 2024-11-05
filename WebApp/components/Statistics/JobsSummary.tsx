import React from 'react'

const JobsSummary = () => {
  
  return (
    <div className="text-center p-4 gap-5 grid grid-cols-4 border border-gray-700 rounded-t-md border-b-0">
      <div className='statistics--jobsCountElement'>Total Jobs: {testData.totalJobs}</div>
      <div className='statistics--jobsCountElement'>Pending Jobs: {testData.pendingJobs}</div>
      <div className='statistics--jobsCountElement'>Completed Jobs: {testData.completedJobs}</div>
      <div className='statistics--jobsCountElement'>In Progress Jobs: {testData.inProgressJobs}</div>
    </div>
  )
}

export default JobsSummary


const testData = {
  totalJobs: 54,
  pendingJobs: 13,
  completedJobs: 5,
  inProgressJobs: 36
}