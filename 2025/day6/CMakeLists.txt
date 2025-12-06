add_executable(day6)
target_sources(day6
    PRIVATE
    day6.cpp)
target_link_libraries(day6 read_file)
target_include_directories(day6 PRIVATE ../utils)
