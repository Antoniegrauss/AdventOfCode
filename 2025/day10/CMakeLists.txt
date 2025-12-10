add_executable(day10)
target_sources(day10
    PRIVATE
    day10.cpp)
target_link_libraries(day10 read_file)
target_include_directories(day10 PRIVATE ../utils)
