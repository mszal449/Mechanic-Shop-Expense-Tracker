import React from 'react'



const JobPage = async ({params} : { params: Promise<{id: number}>}) => {
    const jobId = (await params).id

  return (
    <div>Job id: {jobId}</div>
  )
}

export default JobPage